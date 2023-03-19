// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using System.Security.Claims;

namespace Nocturne.Auth.Core.Services.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        public static ApplicationUser GetApplicationUser(
            this ClaimsPrincipal principal)
        {
            return new ApplicationUser
            {
                Id = GetApplicationUserId(principal),
                Name = principal.FindFirstValue("name") ?? principal.FindFirstValue(ClaimTypes.Name),
                UserName = principal.FindFirstValue(ClaimTypes.Email),
            };
        }

        public static long GetApplicationUserId(
            this ClaimsPrincipal principal)
        {
            return long.Parse(
                principal.FindFirstValue(ClaimTypes.NameIdentifier),
                CultureInfo.InvariantCulture);
        }
    }
}
