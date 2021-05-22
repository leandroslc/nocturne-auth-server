using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly AccessControlService service;

        public PermissionHandler(AccessControlService service)
        {
            this.service = service;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User is null )
            {
                return;
            }

            if (context.User.Identity.IsAuthenticated is false)
            {
                return;
            }

            var access = await service.GetUserAccessControlAsync();

            if (HasPermission(access, requirement))
            {
                context.Succeed(requirement);
            }
        }

        private static bool HasPermission(
            UserAccessControlResponse access,
            PermissionRequirement requirement)
        {
            return requirement.Permissions.Any(
                permission => access.Permissions.Contains(permission));
        }
    }
}
