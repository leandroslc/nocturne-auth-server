using System.Linq;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class AnyPermissionAuthorizationHandler
        : AccessControlAuthorizationHandler<AnyPermissionAuthorizationRequirement>
    {
        public AnyPermissionAuthorizationHandler(
            UserAccessControlService service)
            : base(service)
        {
        }

        protected override bool IsAllowed(
            UserAccessControlResponse access,
            AnyPermissionAuthorizationRequirement requirement)
        {
            return access.Permissions.Any();
        }
    }
}
