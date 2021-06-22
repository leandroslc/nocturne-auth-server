using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Server.Areas.Identity.Emails;

namespace Nocturne.Auth.Server.Configuration
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityEmails(
            this IServiceCollection services)
        {
            services.AddScoped<IdentityEmailService>();

            return services;
        }
    }
}
