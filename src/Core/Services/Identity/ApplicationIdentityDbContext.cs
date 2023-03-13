// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Services.Identity
{
    public class ApplicationIdentityDbContext
        : IdentityUserContext<ApplicationUser, long>
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

            builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins");
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
        }
    }
}
