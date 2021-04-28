using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListApplicationRolesResult
    {
        public bool IsSuccess { get; private set; }

        public bool IsNotFound { get; private set; }

        public string ErrorMessage { get; private set; }

        public IReadOnlyCollection<ListApplicationRolesItem> Roles { get; private set; }

        public static ListApplicationRolesResult Success(
            IReadOnlyCollection<ListApplicationRolesItem> roles)
        {
            return new ListApplicationRolesResult
            {
                IsSuccess = true,
                Roles = roles,
            };
        }

        public static ListApplicationRolesResult NotFound(string description)
        {
            return new ListApplicationRolesResult
            {
                IsNotFound = true,
                ErrorMessage = description,
            };
        }
    }
}
