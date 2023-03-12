// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserCommand
    {
        public AssignRolesToUserCommand()
        {
        }

        public long? UserId { get; set; }

        public IReadOnlyCollection<AssignRolesToUserRole> Roles { get; set; }
    }
}
