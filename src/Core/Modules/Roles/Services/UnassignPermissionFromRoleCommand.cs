// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignPermissionFromRoleCommand
    {
        public long? RoleId { get; set; }

        public long? PermissionId { get; set; }
    }
}
