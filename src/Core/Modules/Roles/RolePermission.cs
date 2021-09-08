// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Permissions;

namespace Nocturne.Auth.Core.Modules.Roles
{
    public class RolePermission
    {
        public RolePermission(
            Role role,
            Permission permission)
        {
            Check.NotNull(role, nameof(role));
            Check.NotNull(permission, nameof(permission));

            PermissionId = permission.Id;
            RoleId = role.Id;
        }

        public RolePermission()
        {
        }

        public long PermissionId { get; private set; }

        public Permission Permission { get; private set; }

        public long RoleId { get; private set; }

        public Role Role { get; private set; }
    }
}
