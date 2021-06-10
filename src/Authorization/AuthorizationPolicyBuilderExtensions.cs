using Nocturne.Auth.Authorization.Requirements;

namespace Microsoft.AspNetCore.Authorization
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
            return builder.AddRequirements(new PermissionAuthorizationRequirement(permissions));
        }

        public static AuthorizationPolicyBuilder RequireCustomRole(
            this AuthorizationPolicyBuilder builder,
            string role)
        {
            return builder.RequireCustomRoles(role);
        }

        public static AuthorizationPolicyBuilder RequireCustomRoles(
            this AuthorizationPolicyBuilder builder,
            params string[] roles)
        {
            return builder.AddRequirements(new RoleAuthorizationRequirement(roles));
        }
    }
}
