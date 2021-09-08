// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ListUsersCommand : PagedCommand<ListUsersItem>
    {
        public string Name { get; set; }

        public string UserName { get; set; }
    }
}
