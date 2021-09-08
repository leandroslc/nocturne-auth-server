// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Hosting;
using Nocturne.Auth.Configuration;

namespace Nocturne.Auth.Server
{
    public class Program
    {
        public static void Main(string[] args)
            => new AppHostBuilder<Startup>(args).Start();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => new AppHostBuilder<Startup>(args).GetHostBuilder();
    }
}
