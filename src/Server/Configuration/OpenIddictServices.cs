using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Server.Configuration
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationServer(
            this OpenIddictBuilder builder,
            IWebHostEnvironment environment)
        {
            builder
                .AddServer(options =>
                {
                    options
                        .SetAuthorizationEndpointUris(AuthorizationEndpoints.Authorize)
                        .SetLogoutEndpointUris(AuthorizationEndpoints.Logout)
                        .SetTokenEndpointUris(AuthorizationEndpoints.Token)
                        .SetUserinfoEndpointUris(AuthorizationEndpoints.UserInfo);

                    options
                        .AllowAuthorizationCodeFlow()
                        .AllowClientCredentialsFlow()
                        .AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    options
                        .RegisterScopes(
                            OpenIddictConstants.Scopes.Email,
                            OpenIddictConstants.Scopes.Profile);

                    options.RequireProofKeyForCodeExchange();

                    var aspNetCoreBuilder = options
                        .UseAspNetCore()
                        .EnableStatusCodePagesIntegration()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough()
                        .EnableTokenEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough();

                    if (environment.IsDevelopment())
                    {
                        options
                            .AddDevelopmentEncryptionCertificate()
                            .AddDevelopmentSigningCertificate();

                        aspNetCoreBuilder.DisableTransportSecurityRequirement();
                    }
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            return builder;
        }
    }
}
