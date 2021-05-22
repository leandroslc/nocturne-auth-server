using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(
            IReadOnlyCollection<string> permissions)
        {
            Permissions = permissions;
        }

        public IReadOnlyCollection<string> Permissions { get; }
    }
}
