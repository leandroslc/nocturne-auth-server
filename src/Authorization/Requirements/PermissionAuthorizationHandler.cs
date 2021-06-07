using System.Linq;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionAuthorizationHandler
        : AccessControlAuthorizationHandler<PermissionAuthorizationRequirement>
    {
        public PermissionAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            UserAccessControlService service)
            : base(httpContextAccessor, service)
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
