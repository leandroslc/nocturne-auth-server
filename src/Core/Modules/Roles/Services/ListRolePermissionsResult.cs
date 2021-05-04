using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListRolePermissionsResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public string ErrorMessage { get; private set; }

        public IReadOnlyCollection<ListRolePermissionsItem> Permissions { get; private set; }

        public static ListRolePermissionsResult Success(
            IReadOnlyCollection<ListRolePermissionsItem> permissions)
        {
            return new ListRolePermissionsResult
            {
                IsSuccess = true,
                Permissions = permissions,
            };
        }

        public static ListRolePermissionsResult NotFound(string description)
        {
            return new ListRolePermissionsResult
            {
                IsNotFound = true,
                ErrorMessage = description,
            };
        }
    }
}
