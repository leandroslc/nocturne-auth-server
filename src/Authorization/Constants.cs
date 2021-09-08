// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;

namespace Nocturne.Auth.Authorization
{
    internal static class Constants
    {
        internal const string PermissionClaim = "permission";

        internal const string RoleClaim = ClaimTypes.Role;
    }
}
