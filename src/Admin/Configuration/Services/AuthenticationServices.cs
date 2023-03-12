// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Configuration.Options;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AuthenticationServices
    {
        private const string AccessDeniedPath = "/error/denied";
        private const string RemoteAuthFailurePath = "/error/remote-auth";

        public static IServiceCollection AddApplicationAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authorizationOptions = configuration
                .GetSection(AuthorizationOptions.Section)
                .Get<AuthorizationOptions>();

            services.AddSingleton(authorizationOptions);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = ApplicationConstants.AuthenticationScheme;
                    options.DefaultChallengeScheme = ApplicationConstants.AuthenticationChallengeScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = CookieNameGenerator.Compute("auth", ApplicationConstants.Identifier);
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ReturnUrlParameter = "returnUrl";
                    options.AccessDeniedPath = AccessDeniedPath;
                })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = authorizationOptions.Authority;
                    options.ClientId = authorizationOptions.ClientId;
                    options.ClientSecret = authorizationOptions.ClientSecret;
                    options.RequireHttpsMetadata = authorizationOptions.RequireHttps;
                    options.AccessDeniedPath = AccessDeniedPath;

                    foreach (var scope in authorizationOptions.Scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    options.GetClaimsFromUserInfoEndpoint = false;
                    options.SaveTokens = false;

                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                    options.DisableTelemetry = true;

                    options.Events.OnRemoteFailure = OnRemoteAuthFailure;

                    options.NonceCookie.Name = "oidc-nonce";
                    options.CorrelationCookie.Name = "oidc-correlation";
                });

            return services;
        }

        private static Task OnRemoteAuthFailure(RemoteFailureContext context)
        {
            context.Response.Redirect(RemoteAuthFailurePath);
            context.HandleResponse();

            return Task.CompletedTask;
        }
    }
}
