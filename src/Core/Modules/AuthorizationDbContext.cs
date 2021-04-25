using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Services.OpenIddict;

namespace Nocturne.Auth.Core.Modules
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("auth");

            ConfigureOpenIdModels(builder);
            ConfigurePermissions(builder);
        }

        private static void ConfigureOpenIdModels(ModelBuilder builder)
        {
            builder.Entity<Application>().ToTable("Applications");
            builder.Entity<Authorization>().ToTable("Authorizations");
            builder.Entity<Scope>().ToTable("Scopes");
            builder.Entity<Token>().ToTable("Tokens");
        }

        private static void ConfigurePermissions(ModelBuilder builder)
        {
            var permissions = builder.Entity<Permission>().ToTable("Permissions");

            permissions.Property(p => p.ConcurrencyToken)
                .HasMaxLength(50)
                .IsConcurrencyToken();

            permissions.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            permissions.Property(p => p.Description)
                .HasMaxLength(400);
        }
    }
}
