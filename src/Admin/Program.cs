// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Admin.Services.Initialization;
using Nocturne.Auth.Configuration;

namespace Nocturne.Auth.Admin
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new AppHostBuilder<Startup>(args);

            if (HasInitializationArg(args))
            {
                var host = builder
                    .ConfigureDefaults(AddInitializationConfig)
                    .Build();

                InitializationService.Run(host.Services);

                return;
            }

            builder.BuildAndStart();
        }

        // Used by external services, like EF CLI
        public static IHostBuilder CreateHostBuilder(string[] args)
            => new AppHostBuilder<Startup>(args).InternalBuilder;

        private static void AddInitializationConfig(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder.AddJsonFile("initialize.json", optional: false);
            });

            webHostBuilder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });
        }

        private static bool HasInitializationArg(string[] args)
            => args.Any(arg => string.Equals(arg, "--init", StringComparison.OrdinalIgnoreCase));
    }
}
