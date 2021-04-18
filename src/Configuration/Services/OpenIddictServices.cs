using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Modules.Applications.Services;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Configuration.Services
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationOpenIddict(
            this IServiceCollection services)
        {
            services.AddScoped<CreateApplicationHandler>();
            services.AddScoped<EditApplicationHandler>();
            services.AddScoped<ViewApplicationHandler>();
            services.AddScoped<ListApplicationsHandler>();

            return services
                .AddOpenIddict()
                .AddCore(options =>
                {
                    options
                        .ReplaceApplicationManager(typeof(CustomOpenIddictApplicationManager<>));

                    options
                        .UseEntityFrameworkCore()
                        .UseDbContext<AuthorizationDbContext>()
                        .ReplaceDefaultEntities<Application, Authorization, Scope, Token, string>();
                });
        }
    }
}
