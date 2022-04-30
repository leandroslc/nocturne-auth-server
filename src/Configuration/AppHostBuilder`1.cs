// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nocturne.Auth.Configuration
{
    public class AppHostBuilder<TStartup>
        where TStartup : class
    {
        private readonly IHostBuilder hostBuilder;

        public AppHostBuilder(string[] args)
        {
            hostBuilder = CreateHostBuilder(args);
        }

        public IHostBuilder InternalBuilder => hostBuilder;

        public AppHostBuilder<TStartup> ConfigureDefaults(Action<IWebHostBuilder> configure)
        {
            hostBuilder.ConfigureWebHostDefaults(configure);

            return this;
        }

        public AppHostBuilder<TStartup> Configure(Action<IHostBuilder> configure)
        {
            configure?.Invoke(hostBuilder);

            return this;
        }

        public IHost Build()
        {
            return hostBuilder.Build();
        }

        public void BuildAndStart()
        {
            var host = Build();

            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Run(host);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.AddJsonFile(
                        "appsettings.local.json",
                        optional: true,
                        reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseStartup<TStartup>();
                })
                .UseSerilog((hostContext, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostContext.Configuration);
                });

            return host;
        }

        private static void Run(IHost host)
        {
            try
            {
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");

                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
