// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Options;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Shared.Helpers;

namespace Nocturne.Auth.Configuration.Services
{
    public static class DataProtectionServices
    {
        private const string SharedApplicationIdentifier = "nocturne-auth-server";

        public static IDataProtectionBuilder AddApplicationDataProtection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new ApplicationDataProtectionOptions();

            configuration
                .GetSection(ApplicationDataProtectionOptions.Section)
                .Bind(options);

            var builder = services
                .AddDataProtection()
                .SetApplicationName(SharedApplicationIdentifier)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(180))
                .PersistKeysToDbContext<DataProtectionDbContext>();

            if (string.IsNullOrWhiteSpace(options.EncryptionCertificateThumbprint) is false)
            {
                var certificate = X509CertificateLocator.FindByThumbprint(
                    options.EncryptionCertificateThumbprint);

                builder.ProtectKeysWithCertificate(certificate);
            }

            return builder;
        }
    }
}
