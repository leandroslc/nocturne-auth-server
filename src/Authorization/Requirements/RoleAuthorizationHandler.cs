using System.Linq;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class RoleAuthorizationHandler
        : AccessControlAuthorizationHandler<RoleAuthorizationRequirement>
    {
        public RoleAuthorizationHandler(
            UserAccessControlService service)
            : base(service)
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
