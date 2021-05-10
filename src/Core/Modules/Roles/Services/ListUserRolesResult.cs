using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListUserRolesResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public string ErrorMessage { get; private set; }

        public IPagedCollection<ListUserRolesItem> Roles { get; private set; }

        public static ListUserRolesResult Success(
            IPagedCollection<ListUserRolesItem> roles)
        {
            return new ListUserRolesResult
            {
                IsSuccess = true,
                Roles = roles,
            };
        }

        public static ListUserRolesResult NotFound(string description)
        {
            return new ListUserRolesResult
            {
                IsNotFound = true,
                ErrorMessage = description,
            };
        }
    }
}
