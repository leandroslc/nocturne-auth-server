using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.OpenIddict;

namespace Nocturne.Auth.Configuration.Services
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationOpenIddict(
            this IServiceCollection services)
        {
            return services
                .AddOpenIddict()
                .AddCore(options =>
                {
                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<OpenIddictDbContext>()
                        .ReplaceDefaultEntities<long>();
                });
        }
    }
}
