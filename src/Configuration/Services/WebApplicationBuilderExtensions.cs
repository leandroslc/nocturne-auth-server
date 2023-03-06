// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Nocturne.Auth.Configuration.Services
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddLocalSettings(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(
                "appsettings.local.json",
                optional: true,
                reloadOnChange: true);

            return builder;
        }

        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder
                .Host.UseSerilog((hostContext, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(hostContext.Configuration);
                });

            return builder;
        }
    }
}
