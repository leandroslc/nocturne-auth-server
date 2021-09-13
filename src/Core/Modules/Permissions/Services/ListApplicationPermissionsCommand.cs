// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ListApplicationPermissionsCommand
    {
        public string ApplicationId { get; set; }

        public string Name { get; set; }
    }
}
