// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public ViewRoleItem Role { get; private set; }

        public static ViewRoleResult Success(Role role)
        {
            return new ViewRoleResult
            {
                IsSuccess = true,
                Role = new ViewRoleItem(role),
            };
        }

        public static ViewRoleResult NotFound()
        {
            return new ViewRoleResult
            {
                IsNotFound = true,
            };
        }
    }
}
