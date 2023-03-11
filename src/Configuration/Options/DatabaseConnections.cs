// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Configuration.Options
{
    public class DatabaseConnections
    {
        public const string Section = "DatabaseConnections";
        public const string MainConnectionName = "Main";

        public DatabaseConnectionOptions Main { get; init; }
    }
}
