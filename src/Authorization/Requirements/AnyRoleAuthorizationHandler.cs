// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class AnyRoleAuthorizationHandler
        : AccessControlAuthorizationHandler<AnyRoleAuthorizationRequirement>
    {
        protected override bool IsAllowed(
            ClaimsPrincipal user,
            AnyRoleAuthorizationRequirement requirement)
        {
            return user.FindFirst(Constants.RoleClaim) != null;
        }
    }
}
