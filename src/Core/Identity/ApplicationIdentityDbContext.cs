using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Identity
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

            builder.Entity<ApplicationUser>().ToTable("IdentityUser");
            builder.Entity<ApplicationRole>().ToTable("IdentityRole");
            builder.Entity<IdentityRoleClaim<long>>().ToTable("IdentityRoleClaim");
            builder.Entity<IdentityUserClaim<long>>().ToTable("IdentityUserClaim");
            builder.Entity<IdentityUserLogin<long>>().ToTable("IdentityUserLogin");
            builder.Entity<IdentityUserRole<long>>().ToTable("IdentityUserRole");
            builder.Entity<IdentityUserToken<long>>().ToTable("IdentityUserToken");
        }
    }
}
