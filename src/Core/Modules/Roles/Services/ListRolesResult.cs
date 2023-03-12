// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListRolesResult
    {
        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get; private set; }

        public IReadOnlyCollection<ListRolesItem> Roles { get; private set; }

        public static ListRolesResult Success(
            IReadOnlyCollection<ListRolesItem> roles)
        {
            return new ListRolesResult
            {
                IsSuccess = true,
                Roles = roles,
            };
        }
    }
}
