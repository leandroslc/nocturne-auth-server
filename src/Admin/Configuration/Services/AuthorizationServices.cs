// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Authorization;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AuthorizationServices
    {
        public static IServiceCollection AddApplicationAuthorization(
            this IServiceCollection services)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Policies.ManageApplications,
                    policy => policy.RequirePermission(Permissions.ApplicationManage));

                options.AddPolicy(
                    Policies.ManageUsers,
                    policy => policy.RequirePermissions(Permissions.UserRolesManage));

                options.AddPolicy(
                    Policies.ManageUserRoles,
                    policy => policy.RequirePermission(Permissions.UserRolesManage));

                options.AddPolicy(
                    Policies.GeneralAccess,
                    policy => policy.RequireAnyPermission());
            });
        }
    }
}
