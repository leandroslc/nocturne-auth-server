// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Server.Areas.Identity.Emails;

namespace Nocturne.Auth.Server.Configuration
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityEmails(
            this IServiceCollection services)
        {
            services.AddScoped<IdentityEmailService>();

            return services;
        }
    }
}
