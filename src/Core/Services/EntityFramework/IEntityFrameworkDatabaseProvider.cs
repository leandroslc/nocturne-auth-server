// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Services.EntityFramework
{
    public interface IEntityFrameworkDatabaseProvider
    {
        public void UseProvider(string connectionString, DbContextOptionsBuilder options);
    }
}
