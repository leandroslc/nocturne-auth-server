using Microsoft.EntityFrameworkCore;
using ApplictionAuthorization = Nocturne.Auth.Core.OpenIddict.Authorization;

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
            builder.Entity<ApplictionAuthorization>().ToTable("Authorizations");
            builder.Entity<Scope>().ToTable("Scopes");
            builder.Entity<Token>().ToTable("Tokens");
        }
    }
}
