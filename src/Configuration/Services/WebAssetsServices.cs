using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Configuration.Services
{
    public static class WebAssetsServices
    {
        public static IServiceCollection AddApplicationWebAssets(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            var options = new WebAssetsOptions();

            configuration.GetSection("Assets").Bind(options);

            var assetsService = new WebAssets(options);

            return services.AddSingleton(assetsService);
        }
    }
}
