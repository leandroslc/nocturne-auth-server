// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewApplicationRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public ViewApplicationRoleItem Role { get; private set; }

        public RoleApplication Application { get; private set; }

        public static ViewApplicationRoleResult Success(Role role, RoleApplication application)
        {
            return new ViewApplicationRoleResult
            {
                IsSuccess = true,
                Role = new ViewApplicationRoleItem(role),
                Application = application,
            };
        }

        public static ViewApplicationRoleResult NotFound()
        {
            return new ViewApplicationRoleResult
            {
                IsNotFound = true,
            };
        }
    }
}
