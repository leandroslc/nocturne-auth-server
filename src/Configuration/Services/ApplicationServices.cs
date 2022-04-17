// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Options;
using Nocturne.Auth.Core;
using Nocturne.Auth.Core.Web;

namespace Nocturne.Auth.Configuration.Services
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationWebAssets(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            var options = new WebAssetsOptions();

            configuration.GetSection("Assets").Bind(options);

            var assetsService = new WebAssets(options);

            return services.AddSingleton(assetsService);
        }

        public static IServiceCollection AddApplicationOptions<TApplicationOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TApplicationOptions : ApplicationOptions
        {
            Check.NotNull(configuration, nameof(configuration));

            services.Configure<TApplicationOptions>(
                configuration.GetSection(ApplicationOptions.Section));

            return services;
        }
    }
}
