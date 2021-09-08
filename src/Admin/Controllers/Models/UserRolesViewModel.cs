// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class UserRolesViewModel
    {
        public UserRolesViewModel(
            long userId,
            IPagedCollection<ListUserRolesItem> roles)
        {
            UserId = userId;
            Roles = roles;
        }

        public long UserId { get; private set; }

        public IPagedCollection<ListUserRolesItem> Roles { get; private set; }
    }
}
