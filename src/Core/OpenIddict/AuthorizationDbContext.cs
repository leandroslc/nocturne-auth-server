using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Services.OpenIddict;

namespace Nocturne.Auth.Core.OpenIddict
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

            builder.Entity<Application>().ToTable("Applications");
            builder.Entity<Authorization>().ToTable("Authorizations");
            builder.Entity<Scope>().ToTable("Scopes");
            builder.Entity<Token>().ToTable("Tokens");
        }
    }
}
