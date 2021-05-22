using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization.Configuration;
using Nocturne.Auth.Authorization.Requirements;
using Nocturne.Auth.Authorization.Services;
using Options = Nocturne.Auth.Authorization.Configuration.AuthorizationOptions;

namespace Nocturne.Auth.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAccessControlService(
            this IServiceCollection services,
            Action<Options> optionsBuilder)
        {
            ConfigureOptions(services, optionsBuilder);

            services.AddScoped<AccessControlService>();
            services.AddScoped<UserAccessControlCacheService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            return services;
        }

        private static void ConfigureOptions(
            IServiceCollection services,
            Action<Options> optionsBuilder)
        {
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }

            var options = new Options();

            optionsBuilder(options);

            services.AddSingleton(new AuthorizationSettings(options));
        }
    }
}
