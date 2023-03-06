// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nocturne.Auth.Infra.SqlServer
{
    public static class InfraSqlServerExtensions
    {
        public static void AddSqlServerDbContext<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
        {
            var assemblyName = typeof(InfraSqlServerExtensions).Assembly.FullName;

            services.AddDbContext<TContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly(assemblyName);
                    });

                optionsAction?.Invoke(options);
            });
        }
    }
}
