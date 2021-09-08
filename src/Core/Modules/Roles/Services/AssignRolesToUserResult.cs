// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserResult
    {
        public bool IsNotFound { get; private set; }

        public bool IsFailure { get; private set; }

        public bool IsSuccess { get; private set; }

        public string ErrorDescription { get; private set; }

        public static AssignRolesToUserResult NotFound(string description)
        {
            return new AssignRolesToUserResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }

        public static AssignRolesToUserResult Fail(string description)
        {
            return new AssignRolesToUserResult
            {
                IsFailure = true,
                ErrorDescription = description,
            };
        }

        public static AssignRolesToUserResult Success()
        {
            return new AssignRolesToUserResult
            {
                IsSuccess = true,
            };
        }
    }
}
