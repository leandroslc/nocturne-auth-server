using System.Linq;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionAuthorizationHandler
        : AccessControlAuthorizationHandler<PermissionAuthorizationRequirement>
    {
        public PermissionAuthorizationHandler(
            UserAccessControlService service)
            : base(service)
        {
        }

        protected override bool IsAllowed(
            UserAccessControlResponse access,
            PermissionAuthorizationRequirement requirement)
        {
            return requirement.Permissions.Any(
                permission => access.Permissions.Contains(permission));
        }
    }
}
