using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Identity;
using Nocturne.Auth.Core.OpenIddict;

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

            services.AddSqlServerDbContext<OpenIddictDbContext>(
                connectionString,
                options =>
                {
                    options.UseOpenIddict<long>();
                });

            return services;
        }
    }
}
