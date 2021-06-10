using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Admin.Configuration.Options;
using Nocturne.Auth.Authorization;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public static class AccessControlServices
    {
        public static IServiceCollection AddApplicationAccessControl(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authorizationOptions = configuration
                .GetSection(AuthorizationOptions.Section)
                .Get<AuthorizationOptions>();

            services
                .AddAccessControlService(options =>
                {
                    options.Authority = authorizationOptions.Authority;
                    options.ClientId = authorizationOptions.ClientId;
                    options.AccessControlEndpoint = "api/access";
                    options.GetAccessToken = (httpContext) => httpContext.GetUserAccessTokenAsync();
                })
                .AddPermissionAuthorizationHandler()
                .AddRoleAuthorizationHandler();

            return services;
        }
    }
}
