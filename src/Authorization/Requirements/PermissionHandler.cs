using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserAccessControlService service;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PermissionHandler(
            IHttpContextAccessor httpContextAccessor,
            UserAccessControlService service)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.service = service;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User is null)
            {
                return;
            }

            if (context.User.Identity.IsAuthenticated is false)
            {
                return;
            }

            var command = new UserAccessControlCommand
            {
                AccessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token"),
                UserIdentifier = context.User.Identity.Name,
            };

            var access = await service.GetUserAccessControlAsync(command);

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
