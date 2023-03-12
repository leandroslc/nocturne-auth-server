// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class DeleteRoleResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public static DeleteRoleResult Success()
        {
            return new DeleteRoleResult
            {
                IsSuccess = true,
            };
        }

        public static DeleteRoleResult NotFound()
        {
            return new DeleteRoleResult
            {
                IsNotFound = true,
            };
        }
    }
}
