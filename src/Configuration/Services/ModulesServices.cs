// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using Nocturne.Auth.Core.Modules.Applications.Services;
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

            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<CreateRoleHandler>();
            services.AddScoped<EditRoleHandler>();
            services.AddScoped<ListRolesHandler>();
            services.AddScoped<ViewRoleHandler>();
            services.AddScoped<DeleteRoleHandler>();

            services.AddScoped<ListUsersHandler>();
            services.AddScoped<ViewUserHandler>();

            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<ListUserRolesHandler>();
            services.AddScoped<AssignRolesToUserHandler>();
            services.AddScoped<UnassignRoleFromUserHandler>();
            services.AddScoped<GetUserAccessHandler>();

            return services;
        }
    }
}
