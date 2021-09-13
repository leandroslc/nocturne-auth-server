// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfraSqlServerExtensions
    {
        public static void AddSqlServerDbContext<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<DbContextOptionsBuilder> optionsAction = null,
            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
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

                        sqlServerOptionsAction?.Invoke(sqlServerOptions);
                    });

                optionsAction?.Invoke(options);
            });
        }
    }
}
