// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ListApplicationPermissionsResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public string ErrorMessage { get; private set; }

        public IReadOnlyCollection<ListApplicationPermissionsItem> Permissions { get; private set; }

        public static ListApplicationPermissionsResult Success(
            IReadOnlyCollection<ListApplicationPermissionsItem> permissions)
        {
            return new ListApplicationPermissionsResult
            {
                IsSuccess = true,
                Permissions = permissions,
            };
        }

        public static ListApplicationPermissionsResult NotFound(string description)
        {
            return new ListApplicationPermissionsResult
            {
                IsNotFound = true,
                ErrorMessage = description,
            };
        }
    }
}
