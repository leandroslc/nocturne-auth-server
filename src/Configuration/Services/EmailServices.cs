using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Email;

namespace Nocturne.Auth.Configuration.Services
{
    public static class EmailServices
    {
        public static IServiceCollection AddApplicationEmail(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Section));

            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }
    }
}
