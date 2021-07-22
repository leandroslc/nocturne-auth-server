using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Shared.Models;

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

            ConfigureUsers(builder);

            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens");
        }

        private static void ConfigureUsers(ModelBuilder builder)
        {
            var user = builder.Entity<ApplicationUser>().ToTable("Users");

            user
                .Property(p => p.Enabled)
                .IsRequired();

            user
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            user
                .Property(p => p.CPF)
                .HasMaxLength(11)
                .HasConversion(to => to.Value, from => new CPF(from))
                .IsRequired();
        }
    }
}
