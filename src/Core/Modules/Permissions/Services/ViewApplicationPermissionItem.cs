// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ViewApplicationPermissionItem
    {
        public ViewApplicationPermissionItem(Permission permission)
        {
            Name = permission.Name;
            Description = permission.Description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
