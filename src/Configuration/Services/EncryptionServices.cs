
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Services.Crypto;

namespace Nocturne.Auth.Configuration.Services
{
    public static class EncryptionServices
    {
        public static IServiceCollection AddApplicationEncryption(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EncryptionOptions>(
                configuration.GetSection(EncryptionOptions.Section));

            services.AddSingleton<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
