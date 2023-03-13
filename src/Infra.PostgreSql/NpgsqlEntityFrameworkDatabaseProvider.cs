// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Services.EntityFramework;

namespace Nocturne.Auth.Infra.PostgreSql
{
    public class NpgsqlEntityFrameworkDatabaseProvider : IEntityFrameworkDatabaseProvider
    {
        public void UseProvider(string connectionString, DbContextOptionsBuilder options)
        {
            var assemblyName = typeof(NpgsqlEntityFrameworkDatabaseProvider).Assembly.FullName;

            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(assemblyName);
                });
        }
    }
}
