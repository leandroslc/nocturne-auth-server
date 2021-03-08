using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.OpenIddict
{
    public class OpenIddictDbContext : DbContext
    {
        public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("openid");

            builder.Entity<OpenIddictEntityFrameworkCoreApplication<long>>().ToTable("Applications");
            builder.Entity<OpenIddictEntityFrameworkCoreAuthorization<long>>().ToTable("Authorizations");
            builder.Entity<OpenIddictEntityFrameworkCoreScope<long>>().ToTable("Scopes");
            builder.Entity<OpenIddictEntityFrameworkCoreToken<long>>().ToTable("Tokens");
        }
    }
}
