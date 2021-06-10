using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public abstract class AccessControlAuthorizationHandler<TRequirement>
        : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        private readonly UserAccessControlService service;

        public AccessControlAuthorizationHandler(
            UserAccessControlService service)
        {
            this.service = service;
        }

        protected abstract bool IsAllowed(UserAccessControlResponse access, TRequirement requirement);

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TRequirement requirement)
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
                UserIdentifier = context.User.Identity.Name,
            };

            var access = await service.GetUserAccessControlAsync(command);

            if (IsAllowed(access, requirement))
            {
                context.Succeed(requirement);
            }
        }
    }
}
