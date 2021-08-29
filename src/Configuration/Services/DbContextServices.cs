using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;

namespace Nocturne.Auth.Configuration.Services
{
    public static class DbContextServices
    {
        private const string MainConnectionStringName = "MainConnection";

        public static IServiceCollection AddApplicationDbContexts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(MainConnectionStringName);

            services.AddSqlServerDbContext<ApplicationIdentityDbContext>(
                connectionString);

            services.AddSqlServerDbContext<AuthorizationDbContext>(
                connectionString,
                options =>
                {
                    options.UseOpenIddict<Application, Authorization, Scope, Token, string>();
                });

            services.AddSqlServerDbContext<DataProtectionDbContext>(
                connectionString);

            return services;
        }
    }
}
