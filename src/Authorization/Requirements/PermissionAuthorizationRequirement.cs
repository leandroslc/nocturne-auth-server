// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(
            IReadOnlyCollection<string> permissions)
        {
            Permissions = permissions;
        }

        public IReadOnlyCollection<string> Permissions { get; }
    }
}
