// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Health;

namespace Nocturne.Auth.Configuration.Services
{
    public static class HealthCheckServices
    {
        public static IServiceCollection AddApplicationHealthChecks(
            this IServiceCollection services)
        {
            services
                .AddTransient<DatabaseConnectionHealthCheck>()
                .AddTransient<DatabaseServerHealthCheck>();

            services
                .AddHealthChecks()
                .AddCheck<DatabaseConnectionHealthCheck>("database connections")
                .AddCheck<DatabaseServerHealthCheck>("database servers");

            return services;
        }
    }
}
