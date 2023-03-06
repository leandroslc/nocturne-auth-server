// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Authorization.Requirements
{
    public abstract class AccessControlAuthorizationHandler<TRequirement>
        : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        protected abstract bool IsAllowed(ClaimsPrincipal user, TRequirement requirement);

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TRequirement requirement)
        {
            if (IsUserAuthenticated(context.User) && IsAllowed(context.User, requirement))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static bool IsUserAuthenticated(ClaimsPrincipal user)
        {
            return user is not null
                && user.Identity.IsAuthenticated;
        }
    }
}
