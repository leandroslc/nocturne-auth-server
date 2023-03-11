// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Options;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Infra.SqlServer;

namespace Nocturne.Auth.Configuration.Services
{
    public static class DbContextServices
    {
        public static IServiceCollection AddApplicationDbContexts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var databaseConnections = new DatabaseConnections
            {
                Main = GetOptions(configuration, DatabaseConnections.MainConnectionName),
            };

            var connectionString = BuildConnectionString(databaseConnections.Main);

            services.AddSqlServerDbContext<ApplicationIdentityDbContext>(
                connectionString);

            services.AddSqlServerDbContext<AuthorizationDbContext>(
                connectionString,
                options =>
                {
                    options.UseOpenIddict<Application, Authorization, Scope, Token, string>();
                });

            services.AddSqlServerDbContext<DataProtectionDbContext>(
                connectionString);

            services.AddSingleton(databaseConnections);

            return services;
        }

        private static DatabaseConnectionOptions GetOptions(IConfiguration configuration, string connectionName)
        {
            var databaseConnections = configuration
                .GetSection(DatabaseConnections.Section);

            return databaseConnections
                .GetSection(connectionName)
                .Get<DatabaseConnectionOptions>();
        }

        private static string BuildConnectionString(DatabaseConnectionOptions options)
        {
            var builder = new DbConnectionStringBuilder
            {
                { "Server", options.Host },
                { "Port", options.Port },
                { "Database", options.Database },
                { "User Id", options.User },
                { "Password", options.Password },
                { "Application Name", options.ApplicationName },
            };

            builder = RemoveEmptyKeysFromConnection(builder);

            return builder.ConnectionString;
        }

        private static DbConnectionStringBuilder RemoveEmptyKeysFromConnection(DbConnectionStringBuilder builder)
        {
            foreach (string key in builder.Keys)
            {
                if (string.IsNullOrWhiteSpace(builder[key] as string))
                {
                    builder.Remove(key);
                }
            }

            return builder;
        }
    }
}
