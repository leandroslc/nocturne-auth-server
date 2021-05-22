using Microsoft.AspNetCore.Authorization;
using Nocturne.Auth.Authorization.Requirements;

namespace Nocturne.Auth.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequirePermission(
            this AuthorizationPolicyBuilder builder,
            string permission)
        {
            return builder.RequirePermissions(permission);
        }

        public static AuthorizationPolicyBuilder RequirePermissions(
            this AuthorizationPolicyBuilder builder,
            params string[] permissions)
        {
            return builder.AddRequirements(new PermissionRequirement(permissions));
        }
    }
}
