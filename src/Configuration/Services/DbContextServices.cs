// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;

namespace Nocturne.Auth.Configuration.Services
{
    public static class DbContextServices
    {
        private const string MainConnection = "Main";

        public static IServiceCollection AddApplicationDbContexts(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = BuildConnectionString(configuration, MainConnection);

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

            return services;
        }

        private static string BuildConnectionString(IConfiguration configuration, string connectionName)
        {
            var databaseConnections = configuration
                .GetSection(DatabaseConnectionOptions.Section);

            var databaseOptions = databaseConnections
                .GetSection(connectionName)
                .Get<DatabaseConnectionOptions>();

            var builder = new DbConnectionStringBuilder
            {
                { "Server", databaseOptions.Host },
                { "Port", databaseOptions.Port },
                { "Database", databaseOptions.Database },
                { "User Id", databaseOptions.User },
                { "Password", databaseOptions.Password },
                { "Application Name", databaseOptions.ApplicationName },
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
