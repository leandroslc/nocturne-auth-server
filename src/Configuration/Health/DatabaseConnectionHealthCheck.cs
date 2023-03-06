// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Configuration.Health
{
    public class DatabaseConnectionHealthCheck : IHealthCheck
    {
        private readonly ApplicationIdentityDbContext applicationIdentityDbContext;
        private readonly AuthorizationDbContext authorizationDbContext;
        private readonly DataProtectionDbContext dataProtectionDbContext;

        public DatabaseConnectionHealthCheck(
            ApplicationIdentityDbContext applicationIdentityDbContext,
            AuthorizationDbContext authorizationDbContext,
            DataProtectionDbContext dataProtectionDbContext)
        {
            this.applicationIdentityDbContext = applicationIdentityDbContext;
            this.authorizationDbContext = authorizationDbContext;
            this.dataProtectionDbContext = dataProtectionDbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            if (await CanConnectAsync(applicationIdentityDbContext) &&
                await CanConnectAsync(authorizationDbContext) &&
                await CanConnectAsync(dataProtectionDbContext))
            {
                return HealthCheckResult.Healthy();
            }

            return new HealthCheckResult(context.Registration.FailureStatus);
        }

        private static Task<bool> CanConnectAsync(DbContext context)
        {
            return context.Database.CanConnectAsync();
        }
    }
}
