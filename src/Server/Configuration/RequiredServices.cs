// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Server.Configuration.Options;
using Nocturne.Auth.Server.Services;

namespace Nocturne.Auth.Server.Configuration
{
    public static class RequiredServices
    {
        public static IServiceCollection AddRequiredApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUserClaimsService, UserClaimsService>();

            services.Configure<AccountOptions>(
                configuration.GetSection(AccountOptions.Section));

            return services;
        }
    }
}
