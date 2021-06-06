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
            var options = new Options();

            optionsBuilder?.Invoke(options);

            services.AddOptions<Options>();
            services.AddHttpClient(Constants.HttpClientName);

            services.AddScoped<AuthorizationSettings>();
            services.AddScoped<UserAccessControlService>();
            services.AddScoped<UserAccessControlCacheService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            return services;
        }
    }
}
