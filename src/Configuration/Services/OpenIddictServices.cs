using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Cryptography;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Configuration.Services
{
    public static class OpenIddictServices
    {
        public static OpenIddictBuilder AddApplicationOpenIddict(
            this IServiceCollection services)
        {
            services.AddSingleton<IClientSecretEncryptionService, ClientSecretEncryptionService>();

            var builder = services
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

            return builder;
        }
    }
}
