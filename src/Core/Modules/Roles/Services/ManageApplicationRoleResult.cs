// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class ManageApplicationRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsDuplicated { get; private set; }

        public bool IsNotFound { get; private set; }

        public bool IsFailure { get; private set; }

        public string ErrorDescription { get; private set; }

        public long RoleId { get; private set; }

        public string ApplicationId { get; private set; }

        public static ManageApplicationRoleResult Success(
            long roleId,
            string applicationId)
        {
            return new ManageApplicationRoleResult
            {
                IsSuccess = true,
                RoleId = roleId,
                ApplicationId = applicationId,
            };
        }

        public static ManageApplicationRoleResult Duplicated(string description)
        {
            return new ManageApplicationRoleResult
            {
                IsDuplicated = true,
                ErrorDescription = description,
            };
        }

        public static ManageApplicationRoleResult NotFound(string description)
        {
            return new ManageApplicationRoleResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }
        public static ManageApplicationRoleResult Fail(string description)
        {
            return new ManageApplicationRoleResult
            {
                IsFailure = true,
                ErrorDescription = description,
            };
        }
    }
}
