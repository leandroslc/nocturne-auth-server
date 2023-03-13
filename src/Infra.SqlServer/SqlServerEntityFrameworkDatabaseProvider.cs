// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Services.EntityFramework;

namespace Nocturne.Auth.Infra.SqlServer
{
    public class SqlServerEntityFrameworkDatabaseProvider : IEntityFrameworkDatabaseProvider
    {
        public void UseProvider(string connectionString, DbContextOptionsBuilder options)
        {
            var assemblyName = typeof(SqlServerEntityFrameworkDatabaseProvider).Assembly.FullName;

            options.UseSqlServer(
                connectionString,
                sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(assemblyName);
                });
        }
    }
}
