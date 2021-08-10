using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Server.Configuration.Options;
using Nocturne.Auth.Server.Services;

namespace Nocturne.Auth.Server.Configuration
{
    public static class RequiredServices
    {
        public static IServiceCollection AddRequiredApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUserClaimsService, UserClaimsService>();

            services.Configure<AccountOptions>(
                configuration.GetSection(AccountOptions.Section));

            return services;
        }
    }
}
