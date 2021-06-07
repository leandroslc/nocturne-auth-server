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
            services.AddOptions<AuthorizationOptions>().Configure(optionsBuilder);
            services.AddHttpClient(Constants.HttpClientName);

            services.AddScoped<AuthorizationSettings>();
            services.AddScoped<UserAccessControlService>();
            services.AddScoped<UserAccessControlCacheService>();

            return new UserAccessControlBuilder(services);
        }
    }
}
