using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules.Applications.Services;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Permissions.Services;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Modules.Users.Services;

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

            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<CreateApplicationRoleHandler>();
            services.AddScoped<EditApplicationRoleHandler>();
            services.AddScoped<ListApplicationRolesHandler>();
            services.AddScoped<ViewApplicationRoleHandler>();
            services.AddScoped<DeleteApplicationRoleHandler>();

            services.AddScoped<IRolePermissionsRepository, RolePermissionsRepository>();
            services.AddScoped<ListRolePermissionsHandler>();
            services.AddScoped<AssignPermissionsToRoleHandler>();
            services.AddScoped<UnassignPermissionFromRoleHandler>();

            services.AddScoped<ListUsersHandler>();
            services.AddScoped<ViewUserHandler>();

            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<ListUserRolesHandler>();

            return services;
        }
    }
}
