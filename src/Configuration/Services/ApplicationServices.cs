// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Configuration.Options;
using Nocturne.Auth.Core;

namespace Nocturne.Auth.Configuration.Services
{
    public static class ApplicationServices
    {
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
