// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nocturne.Auth.Configuration.Options;

namespace Nocturne.Auth.Configuration.Health
{
    public class DatabaseServerHealthCheck : IHealthCheck
    {
        private readonly DatabaseConnections databaseConnections;

        public DatabaseServerHealthCheck(DatabaseConnections databaseConnections)
        {
            this.databaseConnections = databaseConnections;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var result = Check(databaseConnections.Main)
                ? HealthCheckResult.Healthy()
                : new HealthCheckResult(context.Registration.FailureStatus);

            return Task.FromResult(result);
        }

        private static bool Check(DatabaseConnectionOptions databaseOptions)
        {
            var (host, port) = GetAddressFromOptions(databaseOptions);

            try
            {
                using (new TcpClient(host, port))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private static (string, int) GetAddressFromOptions(DatabaseConnectionOptions databaseOptions)
        {
            if (string.IsNullOrWhiteSpace(databaseOptions.Port))
            {
                var fragments = databaseOptions.Host.Split(',', ':');
                var host = fragments[0];
                var port = fragments.Length > 1 ? int.Parse(fragments[1], CultureInfo.InvariantCulture) : 0;

                return (host, port);
            }

            return (databaseOptions.Host, int.Parse(databaseOptions.Port, CultureInfo.InvariantCulture));
        }
    }
}
