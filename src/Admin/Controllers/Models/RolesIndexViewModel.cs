// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Services;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class RolesIndexViewModel
    {
        public RolesIndexViewModel(
            IReadOnlyCollection<ListRolesItem> roles,
            ListRolesCommand query)
        {
            Roles = roles;
            Query = query;
        }

        public IReadOnlyCollection<ListRolesItem> Roles { get; private set; }

        public ListRolesCommand Query { get; }
    }
}
