using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Server.Services;

namespace Nocturne.Auth.Server.Configuration
{
    public static class RequiredServices
    {
        public static IServiceCollection AddRequiredApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserClaimsService, UserClaimsService>();

            return services;
        }
    }
}
