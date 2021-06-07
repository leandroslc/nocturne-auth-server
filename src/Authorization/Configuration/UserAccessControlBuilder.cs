using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Authorization.Requirements;

namespace Nocturne.Auth.Authorization.Configuration
{
    public sealed class UserAccessControlBuilder
    {
        private readonly IServiceCollection services;

        public UserAccessControlBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public UserAccessControlBuilder AddPermissionAuthorizationHandler()
        {
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            return this;
        }
    }
}
