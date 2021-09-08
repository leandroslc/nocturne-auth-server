// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignRoleFromUserCommand
    {
        public long? UserId { get; set; }

        public long? RoleId { get; set; }
    }
}
