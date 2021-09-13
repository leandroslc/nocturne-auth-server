// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Server.Configuration.Options;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Server.Configuration
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationServer(
            this OpenIddictBuilder builder,
            IConfiguration configuration)
        {
            var serverOptions = GetIdServerOptions(configuration);

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

                    options.AddCertificates(serverOptions);

                    if (serverOptions.DisableTransportSecurityRequirement)
                    {
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

        private static OpenIdServerOptions GetIdServerOptions(IConfiguration configuration)
        {
            var options = new OpenIdServerOptions();

            configuration.GetSection(OpenIdServerOptions.Section).Bind(options);

            return options;
        }

        private static OpenIddictServerBuilder AddCertificates(
            this OpenIddictServerBuilder builder,
            OpenIdServerOptions options)
        {
            if (options.UseDevelopmentCertificates)
            {
                builder
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
            }
            else
            {
                builder
                    .AddEncryptionCertificate(options.EncryptionCertificateThumbprint)
                    .AddSigningCertificate(options.SigningCertificateThumbprint);
            }

            return builder;
        }
    }
}
