// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignRoleFromUserResult
    {
        public bool IsNotFound { get; private set; }

        public bool IsSuccess { get; private set; }

        public string ErrorDescription { get; private set; }

        public static UnassignRoleFromUserResult NotFound(string description)
        {
            return new UnassignRoleFromUserResult
            {
                IsNotFound = true,
                ErrorDescription = description,
            };
        }

        public static UnassignRoleFromUserResult Success()
        {
            return new UnassignRoleFromUserResult
            {
                IsSuccess = true,
            };
        }
    }
}
