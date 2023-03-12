// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class ManageRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsDuplicated { get; private set; }

        public bool IsNotFound { get; private set; }

        public bool IsFailure { get; private set; }

        public string ErrorDescription { get; private set; }

        public long RoleId { get; private set; }

        public string ApplicationId { get; private set; }

        public static ManageRoleResult Success(long roleId)
        {
            return new ManageRoleResult
            {
                IsSuccess = true,
                RoleId = roleId,
            };
        }

        public static ManageRoleResult Duplicated(string description)
        {
            return new ManageRoleResult
            {
                IsDuplicated = true,
                ErrorDescription = description,
            };
        }

        public static ManageRoleResult NotFound(string description)
        {
            return new ManageRoleResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }

        public static ManageRoleResult Fail(string description)
        {
            return new ManageRoleResult
            {
                IsFailure = true,
                ErrorDescription = description,
            };
        }
    }
}
