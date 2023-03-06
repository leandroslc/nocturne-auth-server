// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nocturne.Auth.Configuration.Services
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder AddLocalSettings(this IHostBuilder builder)
        {
            return builder
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.AddJsonFile(
                        "appsettings.local.json",
                        optional: true,
                        reloadOnChange: true);
                });
        }

        public static IHostBuilder AddWebHostConfiguration(this IHostBuilder builder)
        {
            return builder
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                });
        }

        public static IHostBuilder AddLogging(this IHostBuilder builder)
        {
            return builder
                .UseSerilog((hostContext, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostContext.Configuration);
                });
        }
    }
}
