// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("auth");

            ConfigureOpenIdModels(modelBuilder);
            ConfigureRoles(modelBuilder);
            ConfigureUserRoles(modelBuilder);
        }

        private static void ConfigureOpenIdModels(ModelBuilder builder)
        {
            builder.Entity<Application>().ToTable("applications");
            builder.Entity<Authorization>().ToTable("authorizations");
            builder.Entity<Scope>().ToTable("scopes");
            builder.Entity<Token>().ToTable("tokens");
        }

        private static void ConfigureRoles(ModelBuilder builder)
        {
            var roles = builder.Entity<Role>().ToTable("roles");

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
        }

        private static void ConfigureUserRoles(ModelBuilder builder)
        {
            var userRoles = builder.Entity<UserRole>().ToTable("user_roles");

            userRoles.HasKey(p => new { p.UserId, p.RoleId });

            userRoles
                .HasOne(p => p.Role)
                .WithMany()
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
