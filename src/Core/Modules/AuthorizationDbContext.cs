using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Modules.Roles;
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
            ConfigureRoles(builder);
            ConfigureRolePermissions(builder);
            ConfigureUserRoles(builder);
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

            permissions
                .Property(p => p.ConcurrencyToken)
                .HasMaxLength(50)
                .IsConcurrencyToken();

            permissions
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            permissions
                .Property(p => p.Description)
                .HasMaxLength(400);

            permissions
                .HasOne(p => p.Application)
                .WithMany()
                .HasForeignKey(p => p.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureRoles(ModelBuilder builder)
        {
            var roles = builder.Entity<Role>().ToTable("Roles");

            roles
                .Property(p => p.ConcurrencyToken)
                .HasMaxLength(50)
                .IsConcurrencyToken();

            roles
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            roles
                .Property(p => p.Description)
                .HasMaxLength(400);

            roles
                .HasOne(p => p.Application)
                .WithMany()
                .HasForeignKey(p => p.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureRolePermissions(ModelBuilder builder)
        {
            var rolePermissions = builder.Entity<RolePermission>().ToTable("RolePermissions");

            rolePermissions.HasKey(p => new { p.RoleId, p.PermissionId });

            rolePermissions
                .HasOne(p => p.Permission)
                .WithMany()
                .HasForeignKey(p => p.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            rolePermissions
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureUserRoles(ModelBuilder builder)
        {
            var userRoles = builder.Entity<UserRole>().ToTable("UserRoles");

            userRoles.HasKey(p => new { p.UserId, p.RoleId });

            userRoles
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
