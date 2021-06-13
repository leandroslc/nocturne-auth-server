using System.Linq;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class AnyRoleAuthorizationHandler
        : AccessControlAuthorizationHandler<AnyRoleAuthorizationRequirement>
    {
        public AnyRoleAuthorizationHandler(
            UserAccessControlService service)
            : base(service)
        {
        }

        protected override bool IsAllowed(
            UserAccessControlResponse access,
            AnyRoleAuthorizationRequirement requirement)
        {
            return access.Roles.Any();
        }
    }
}
