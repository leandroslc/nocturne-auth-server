using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nocturne.Auth.Authorization.Configuration;
using Nocturne.Auth.Authorization.Services;

namespace Nocturne.Auth.Authorization.Requirements
{
    public abstract class AccessControlAuthorizationHandler<TRequirement>
        : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        private readonly UserAccessControlService service;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccessControlAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            UserAccessControlService service)
        {
            this.httpContextAccessor = httpContextAccessor;
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
                AccessToken = await httpContextAccessor.HttpContext.GetTokenAsync(Constants.AccessTokenName),
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
