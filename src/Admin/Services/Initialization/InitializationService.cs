using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Core.OpenIddict;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Admin.Services.Initialization
{
    public class InitializationService
    {
        private readonly IServiceProvider serviceProvider;
        private ILogger logger;
        private InitializationData data;

        public InitializationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public static void Run(IServiceProvider services)
        {
            new InitializationService(services).Run().Wait();
        }

        private async Task Run()
        {
            logger = CreateLogger();
            data = GetInitializationData();

            using var serviceScope = serviceProvider.CreateScope();

            var services = serviceScope.ServiceProvider;

            await ApplyOpenIddictMigrations(services);

            await CreateScopes(services);
        }

        private async Task ApplyOpenIddictMigrations(IServiceProvider services)
        {
            logger.LogInformation($"Applying database migrations for \"{nameof(OpenIddictDbContext)}\"");

            var context = services.GetRequiredService<OpenIddictDbContext>();
            await context.Database.MigrateAsync();
        }

        private ILogger CreateLogger()
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            return loggerFactory.CreateLogger<InitializationService>();
        }

        private InitializationData GetInitializationData()
        {
            logger.LogInformation("Getting initialization data");

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var data = new InitializationData();

            configuration
                .GetSection(InitializationData.Section)
                .Bind(data);

            return data;
        }

        private async Task CreateScopes(IServiceProvider services)
        {
            logger.LogInformation("Creating scopes");

            var scopeManager = services.GetRequiredService<IOpenIddictScopeManager>();

            foreach (var scope in data.Scopes)
            {
                if (await scopeManager.FindByNameAsync(scope.Name) is not null)
                {
                    logger.LogInformation($"Scope \"{scope.Name}\" already exists");

                    continue;
                }

                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = scope.Name,
                    Description = scope.Description,
                    DisplayName = scope.DisplayName,
                };

                foreach (var displayName in scope.DisplayNames)
                {
                    descriptor.DisplayNames.Add(
                        new CultureInfo(displayName.Key),
                        displayName.Value);
                }

                foreach (var description in scope.Descriptions)
                {
                    descriptor.Descriptions.Add(
                        new CultureInfo(description.Key),
                        description.Value);
                }

                foreach (var resource in scope.Resources)
                {
                    descriptor.Resources.Add(resource);
                }

                await scopeManager.CreateAsync(descriptor);

                logger.LogInformation($"Scope \"{scope.Name}\" created");
            }
        }
    }
}
