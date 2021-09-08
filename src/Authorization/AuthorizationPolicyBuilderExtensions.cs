// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Authorization.Requirements;

namespace Microsoft.AspNetCore.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequirePermission(
            this AuthorizationPolicyBuilder builder,
            string permission)
        {
            return builder.RequirePermissions(permission);
        }

        public static AuthorizationPolicyBuilder RequirePermissions(
            this AuthorizationPolicyBuilder builder,
            params string[] permissions)
        {
            return builder.AddRequirements(new PermissionAuthorizationRequirement(permissions));
        }

        public static AuthorizationPolicyBuilder RequireAnyPermission(
            this AuthorizationPolicyBuilder builder)
        {
            return builder.AddRequirements(new AnyPermissionAuthorizationRequirement());
        }

        public static AuthorizationPolicyBuilder RequireAnyRole(
            this AuthorizationPolicyBuilder builder)
        {
            return builder.AddRequirements(new AnyRoleAuthorizationRequirement());
        }
    }
}
