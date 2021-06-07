using System.Linq;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class RoleAuthorizationHandler
        : AccessControlAuthorizationHandler<RoleAuthorizationRequirement>
    {
        public RoleAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            UserAccessControlService service)
            : base(httpContextAccessor, service)
        {
        }

        protected override bool IsAllowed(
            UserAccessControlResponse access,
            RoleAuthorizationRequirement requirement)
        {
            return requirement.Roles.Any(
                role => access.Roles.Contains(role));
        }
    }
}
