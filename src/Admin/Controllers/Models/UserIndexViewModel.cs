// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class UserIndexViewModel
    {
        public ListUsersCommand Query { get; set; }

        public IPagedCollection<ListUsersItem> Users { get; set; }
    }
}
