using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.OpenIddict;
using Nocturne.Auth.Core.OpenIddict.Applications;

namespace Nocturne.Auth.Configuration.Services
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationOpenIddict(
            this IServiceCollection services)
        {
            services.AddScoped<CreateApplicationHandler>();

            return services
                .AddOpenIddict()
                .AddCore(options =>
                {
                    options
                        .ReplaceApplicationManager(typeof(CustomOpenIddictApplicationManager<>));

                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<OpenIddictDbContext>()
                        .ReplaceDefaultEntities<long>();
                });
        }
    }
}
