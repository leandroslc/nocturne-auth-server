// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Configuration;

namespace Nocturne.Auth.Server
{
    public static class Program
    {
        public static void Main(string[] args)
            => new AppHostBuilder<Startup>(args).BuildAndStart();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => new AppHostBuilder<Startup>(args).InternalBuilder;
    }
}
