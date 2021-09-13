// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class AnyPermissionAuthorizationHandler
        : AccessControlAuthorizationHandler<AnyPermissionAuthorizationRequirement>
    {
        protected override bool IsAllowed(
            ClaimsPrincipal user,
            AnyPermissionAuthorizationRequirement requirement)
        {
            return user.FindFirst(Constants.PermissionClaim) != null;
        }
    }
}
