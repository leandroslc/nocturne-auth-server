using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(
            IReadOnlyCollection<string> permissions)
        {
            Permissions = permissions;
        }

        public IReadOnlyCollection<string> Permissions { get; }
    }
}
