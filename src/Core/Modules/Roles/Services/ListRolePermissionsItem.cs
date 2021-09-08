// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListRolePermissionsItem
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ApplicationId { get; set; }

        public string ApplicationName { get; set; }
    }
}
