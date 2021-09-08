// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListUserRolesCommand : PagedCommand<ListUserRolesItem>
    {
        public string Name { get; set; }

        public long? UserId { get; set; }
    }
}
