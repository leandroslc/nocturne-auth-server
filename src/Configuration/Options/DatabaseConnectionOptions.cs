// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Configuration
{
    public class DatabaseConnectionOptions
    {
        public const string Section = "DatabaseConnections";

        public string Host { get; set; }

        public string Port { get; set; }

        public string Database { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string ApplicationName { get; set; }
    }
}
