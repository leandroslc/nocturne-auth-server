// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class UserIndexViewModel
    {
        public UserIndexViewModel(
            IPagedCollection<ListUsersItem> users,
            ListUsersCommand query)
        {
            Users = users;
            Query = query;
        }

        public ListUsersCommand Query { get; }

        public IPagedCollection<ListUsersItem> Users { get; }
    }
}
