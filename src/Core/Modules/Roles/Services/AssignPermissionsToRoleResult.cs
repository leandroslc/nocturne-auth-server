// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignPermissionsToRoleResult
    {
        public bool IsNotFound { get; private set; }

        public bool IsFailure { get; private set; }

        public bool IsSuccess { get; private set; }

        public string ErrorDescription { get; private set; }

        public static AssignPermissionsToRoleResult NotFound(string description)
        {
            return new AssignPermissionsToRoleResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }

        public static AssignPermissionsToRoleResult Fail(string description)
        {
            return new AssignPermissionsToRoleResult
            {
                IsFailure = true,
                ErrorDescription = description,
            };
        }

        public static AssignPermissionsToRoleResult Success()
        {
            return new AssignPermissionsToRoleResult
            {
                IsSuccess = true,
            };
        }
    }
}
