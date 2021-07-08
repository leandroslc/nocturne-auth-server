using System.Linq;
using System.Security.Claims;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionAuthorizationHandler
        : AccessControlAuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override bool IsAllowed(
            ClaimsPrincipal user,
            PermissionAuthorizationRequirement requirement)
        {
            return requirement.Permissions.Any(
                permission => user.HasClaim(Constants.PermissionClaim, permission));
        }
    }
}
