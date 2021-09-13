// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ListUsersItem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }
    }
}
