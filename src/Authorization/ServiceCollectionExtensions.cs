using System;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization.Configuration;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static UserAccessControlBuilder AddAccessControlService(
            this IServiceCollection services,
            Action<AuthorizationOptions> optionsBuilder)
        {
            services.AddAuthorizationSettings(optionsBuilder);
            services.AddHttpClient(Constants.HttpClientName);
            services.AddHttpContextAccessor();

            services.AddScoped<UserAccessControlService>();
            services.AddScoped<UserAccessControlCacheService>();

            return new UserAccessControlBuilder(services);
        }

        private static void AddAuthorizationSettings(
            this IServiceCollection services,
            Action<AuthorizationOptions> optionsBuilder)
        {
            var options = new AuthorizationOptions();

            optionsBuilder?.Invoke(options);

            var settings = new AuthorizationSettings(options);

            services.AddSingleton(settings);
        }
    }
}
