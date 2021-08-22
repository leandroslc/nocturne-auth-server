using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Services.DataProtection;

namespace Nocturne.Auth.Configuration.Services
{
    public static class DataProtectionServices
    {
        private const string SharedApplicationIdentifier = "nocturne-auth-server";

        public static IServiceCollection AddApplicationDataProtection(
            IServiceCollection services)
        {
            services
                .AddDataProtection()
                .SetApplicationName(SharedApplicationIdentifier)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(180))
                .PersistKeysToDbContext<DataProtectionDbContext>();

            return services;
        }
    }
}
