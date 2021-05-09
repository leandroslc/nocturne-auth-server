using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Services.Identity
{
    public class ApplicationIdentityDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");

            var user = builder.Entity<ApplicationUser>().ToTable("Users");

            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens");

            user
                .Property(p => p.Enabled)
                .IsRequired();

            user
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
