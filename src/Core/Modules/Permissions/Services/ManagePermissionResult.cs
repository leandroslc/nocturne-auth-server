// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class ManagePermissionResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsDuplicated { get; private set; }

        public bool IsNotFound { get; private set; }

        public bool IsFailure { get; private set; }

        public string ErrorDescription { get; private set; }

        public long PermissionId { get; private set; }

        public static ManagePermissionResult Success(long permissionId)
        {
            return new ManagePermissionResult
            {
                IsSuccess = true,
                PermissionId = permissionId,
            };
        }

        public static ManagePermissionResult Duplicated(string description)
        {
            return new ManagePermissionResult
            {
                IsDuplicated = true,
                ErrorDescription = description,
            };
        }

        public static ManagePermissionResult NotFound(string description)
        {
            return new ManagePermissionResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }
        public static ManagePermissionResult Fail(string description)
        {
            return new ManagePermissionResult
            {
                IsFailure = true,
                ErrorDescription = description,
            };
        }
    }
}
