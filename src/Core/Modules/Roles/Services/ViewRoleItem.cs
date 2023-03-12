// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewRoleItem
    {
        public ViewRoleItem(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
