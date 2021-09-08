// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AccessControlServices
    {
        public static IServiceCollection AddApplicationAccessControl(
            this IServiceCollection services)
        {
            services
                .AddAccessControlService()
                .AddPermissionAuthorizationHandler()
                .AddRoleAuthorizationHandler();

            return services;
        }
    }
}
