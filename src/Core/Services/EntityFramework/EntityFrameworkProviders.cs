// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Services.EntityFramework
{
    public class EntityFrameworkProviders : Dictionary<string, IEntityFrameworkDatabaseProvider>
    {
        public void Use(
            string providerName,
            string connectionString,
            DbContextOptionsBuilder options)
        {
            if (TryGetValue(providerName, out var provider) is false)
            {
                throw new InvalidOperationException($"Database provider {providerName} not found");
            }

            provider.UseProvider(connectionString, options);
        }
    }
}
