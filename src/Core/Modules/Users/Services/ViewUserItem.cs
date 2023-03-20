// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ViewUserItem
    {
        public ViewUserItem(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.Name;
            Enabled = user.Enabled;
            UserName = user.UserName;
        }

        public long Id { get; }

        public string Name { get; }

        public string UserName { get; }

        public bool Enabled { get; }
    }
}
