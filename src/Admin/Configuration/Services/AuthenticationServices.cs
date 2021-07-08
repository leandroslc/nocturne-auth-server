using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Configuration.Options;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AuthenticationServices
    {
        private const string AccessDeniedPath = "/error/denied";

        public static IServiceCollection AddApplicationAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authorizationOptions = configuration
                .GetSection(AuthorizationOptions.Section)
                .Get<AuthorizationOptions>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
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

                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";

                    options.DisableTelemetry = true;

                    options.NonceCookie.Name = "oidc-nonce";
                    options.CorrelationCookie.Name = "oidc-correlation";
                });

            return services;
        }
    }
}
