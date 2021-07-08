using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AccessControlServices
    {
        public static IServiceCollection AddApplicationAccessControl(
            this IServiceCollection services)
        {
            services
                .AddAccessControlService()
                .AddPermissionAuthorizationHandler()
                .AddRoleAuthorizationHandler();

            return services;
        }
    }
}
