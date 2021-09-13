// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class IAuthorizationServiceExtensions
    {
        public static async Task<bool> HasAccessAsync(
            this IAuthorizationService authorizationService,
            ClaimsPrincipal user,
            string policy)
        {
            return (await authorizationService.AuthorizeAsync(user, policy)).Succeeded;
        }
    }
}
