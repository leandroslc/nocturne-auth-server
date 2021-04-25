using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules.Applications.Services;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Permissions.Services;

namespace Nocturne.Auth.Configuration.Services
{
    public static class ModulesServices
    {
        public static IServiceCollection AddApplicationModules(
            this IServiceCollection services)
        {
            services.AddScoped<CreateApplicationHandler>();
            services.AddScoped<EditApplicationHandler>();
            services.AddScoped<ViewApplicationHandler>();
            services.AddScoped<ListApplicationsHandler>();

            services.AddScoped<IPermissionsRepository, PermissionsRepository>();
            services.AddScoped<CreatePermissionHandler>();
            services.AddScoped<EditPermissionHandler>();
            services.AddScoped<ListApplicationPermissionsHandler>();
            services.AddScoped<ViewApplicationPermissionHandler>();
            services.AddScoped<DeleteApplicationPermissionHandler>();

            return services;
        }
    }
}
