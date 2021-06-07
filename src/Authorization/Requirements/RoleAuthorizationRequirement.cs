using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public RoleAuthorizationRequirement(
            IReadOnlyCollection<string> roles)
        {
            Roles = roles;
        }

        public IReadOnlyCollection<string> Roles { get; }
    }
}
