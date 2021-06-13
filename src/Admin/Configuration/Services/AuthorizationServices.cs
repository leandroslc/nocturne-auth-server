using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Admin.Configuration.Constants;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AuthorizationServices
    {
        public static IServiceCollection AddApplicationAuthorization(
            this IServiceCollection services)
        {
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ManageApplications,
                    policy => policy.RequirePermission(Permissions.ApplicationManage));

                options.AddPolicy(Policies.ManageUserRoles,
                    policy => policy.RequirePermission(Permissions.UserRolesManage));

                options.AddPolicy(Policies.GeneralAccess,
                    policy => policy.RequireAnyPermission());
            });
        }
    }
}
