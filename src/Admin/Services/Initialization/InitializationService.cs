// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Permissions.Services;
using Nocturne.Auth.Core.Modules.Roles;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict.Services;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Admin.Services.Initialization
{
    public class InitializationService
    {
        public InitializationService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        private IServiceProvider ServiceProvider { get; }

        private ILogger Logger { get; set; }

        private InitializationData Data { get; set; }

        public static void Run(IServiceProvider services)
        {
            new InitializationService(services).Run().Wait();
        }

        private async Task Run()
        {
            Logger = CreateLogger();
            Data = GetInitializationData();

            using var serviceScope = ServiceProvider.CreateScope();

            var services = serviceScope.ServiceProvider;

            await ApplyAuthorizationMigrations(services);
            await ApplyIdentityMigrations(services);
            await ApplyDataProtectionMigrations(services);

            await CreateScopes(services);

            var adminApplicationId = await CreateAdminApplication(services);
            var adminRoleId = await CreateAdminRole(services, adminApplicationId);
            await CreateAdminUser(services, adminRoleId);
        }

        private Task ApplyAuthorizationMigrations(IServiceProvider services)
            => ApplyMigrations<AuthorizationDbContext>(services);

        private Task ApplyIdentityMigrations(IServiceProvider services)
            => ApplyMigrations<ApplicationIdentityDbContext>(services);

        private Task ApplyDataProtectionMigrations(IServiceProvider services)
            => ApplyMigrations<DataProtectionDbContext>(services);

        private async Task ApplyMigrations<TContext>(
            IServiceProvider services)
            where TContext : DbContext
        {
            var contextName = typeof(TContext).Name;

            Logger.LogInformation("Applying database migrations for {context}", contextName);

            var context = services.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();
        }

        private ILogger CreateLogger()
        {
            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            return loggerFactory.CreateLogger<InitializationService>();
        }

        private InitializationData GetInitializationData()
        {
            Logger.LogInformation("Getting initialization data");

            var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

            var data = new InitializationData();

            configuration
                .GetSection(InitializationData.Section)
                .Bind(data);

            return data;
        }

        private async Task<string> CreateAdminApplication(IServiceProvider services)
        {
            if (Data.AdminApplication is null)
            {
                throw new InvalidOperationException("No application provided");
            }

            var applicationManager = services.GetRequiredService<IOpenIddictApplicationManager>();
            var clientBuilderService = services.GetRequiredService<IClientBuilderService>();

            var applicationDescriptor = Data.AdminApplication;
            applicationDescriptor.ClientId = clientBuilderService.GenerateClientId();
            applicationDescriptor.ClientSecret = clientBuilderService.GenerateClientSecret();

            var registeredApplication = await applicationManager
                .FindByClientIdAsync(applicationDescriptor.DisplayName);

            if (registeredApplication is not null)
            {
                Logger.LogInformation("Application {displayName} already exists", applicationDescriptor.DisplayName);

                return await applicationManager.GetIdAsync(registeredApplication);
            }

            registeredApplication = await applicationManager.CreateAsync(applicationDescriptor);

            Logger.LogInformation(
                "Created application {displayName} " +
                "\"client_id: {clientId}\" " +
                "\"client_secrect: {clientSecret}\"",
                applicationDescriptor.DisplayName,
                applicationDescriptor.ClientId,
                applicationDescriptor.ClientSecret);

            Logger.LogWarning(
                "Save the client id and secret in a safe place. " +
                "They will be used in your configuration");

            return await applicationManager.GetIdAsync(registeredApplication);
        }

        private async Task CreateScopes(IServiceProvider services)
        {
            if (Data.Scopes is null)
            {
                Logger.LogInformation("No scopes provided");

                return;
            }

            Logger.LogInformation("Creating scopes");

            var scopeManager = services.GetRequiredService<IOpenIddictScopeManager>();

            foreach (var scope in Data.Scopes)
            {
                if (await scopeManager.FindByNameAsync(scope.Name) is not null)
                {
                    Logger.LogInformation("Scope {name} already exists", scope.Name);

                    continue;
                }

                await scopeManager.CreateAsync(scope);

                Logger.LogInformation("Created scope {name}", scope.Name);
            }
        }

        private async Task<long> CreateAdminRole(
            IServiceProvider services,
            string adminApplicationId)
        {
            Logger.LogInformation("Creating permissions for {adminApplicationId}", adminApplicationId);

            var applicationManagePermissionId = await CreatePermission(
                services,
                adminApplicationId,
                Permissions.ApplicationManage,
                "View, create and manage all applications");

            var userRolesManagePermissionId = await CreatePermission(
                services,
                adminApplicationId,
                Permissions.UserRolesManage,
                "Assign and unassign users' roles");

            return await CreateRole(
                services,
                adminApplicationId,
                UserRoles.Administrator,
                "Provides full access to all features of the system",
                applicationManagePermissionId,
                userRolesManagePermissionId);
        }

        private async Task<long> CreatePermission(
            IServiceProvider services,
            string applicationId,
            string name,
            string description)
        {
            var createPermissionHandler = services.GetRequiredService<CreatePermissionHandler>();

            var permission = await FindPermission(services, applicationId, name);

            if (permission is not null)
            {
                Logger.LogInformation("Permission {name} of {applicationId} already exists", name, applicationId);

                return permission.Id;
            }

            var command = new CreatePermissionCommand
            {
                ApplicationId = applicationId,
                Name = name,
                Description = description,
            };

            var result = await createPermissionHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                Logger.LogInformation("Created permission {name} for {applicationId}", name, applicationId);

                return result.PermissionId;
            }

            throw new InvalidOperationException(
                $"Error while creating permission {name} for {applicationId}: {result.ErrorDescription}");
        }

        private static async Task<Permission> FindPermission(
            IServiceProvider services,
            string applicationId,
            string name)
        {
            var repository = services.GetRequiredService<IPermissionsRepository>();

            var permissions = await repository.QueryByApplication(
                applicationId,
                q => q.Where(p => p.Name == name));

            return permissions.FirstOrDefault();
        }

        private async Task<long> CreateRole(
            IServiceProvider services,
            string applicationId,
            string name,
            string description,
            params long[] permissionIds)
        {
            var createRoleHandler = services.GetRequiredService<CreateApplicationRoleHandler>();

            var role = await FindRole(services, applicationId, name);

            if (role is not null)
            {
                Logger.LogInformation("Role {name} of {applicationId} already exists", name, applicationId);

                await AssignPermissionsToRole(services, role.Id, permissionIds);

                return role.Id;
            }

            var command = new CreateApplicationRoleCommand
            {
                ApplicationId = applicationId,
                Name = name,
                Description = description,
            };

            var result = await createRoleHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                Logger.LogInformation("Created permission {name} for {applicationId}", name, applicationId);

                await AssignPermissionsToRole(services, result.RoleId, permissionIds);

                return result.RoleId;
            }

            throw new InvalidOperationException(
                $"Error while creating permission {name} for {applicationId}: {result.ErrorDescription}");
        }

        private static async Task<Role> FindRole(
            IServiceProvider services,
            string applicationId,
            string name)
        {
            var repository = services.GetRequiredService<IRolesRepository>();

            var roles = await repository.QueryByApplication(
                applicationId,
                q => q.Where(r => r.Name == name));

            return roles.FirstOrDefault();
        }

        private async Task AssignPermissionsToRole(
            IServiceProvider services,
            long roleId,
            IReadOnlyCollection<long> permissionIds)
        {
            var assignPermissionHandler = services.GetRequiredService<AssignPermissionsToRoleHandler>();

            var command = new AssignPermissionsToRoleCommand
            {
                RoleId = roleId,
                Permissions = permissionIds.Select(id => new AssignPermissionsToRolePermission
                {
                    Id = id,
                    Selected = true,
                })
                .ToArray(),
            };

            var result = await assignPermissionHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                Logger.LogInformation("Assigned permissions to role {roleId}", roleId);
            }
            else
            {
                Logger.LogWarning($"Failed to assign permissions to role {roleId}. {result.ErrorDescription}");
            }
        }

        private async Task CreateAdminUser(IServiceProvider services, params long[] roleIds)
        {
            if (Data.AdminUser is null)
            {
                Logger.LogWarning("No admin user specified");

                return;
            }

            var userId = await CreateUser(services, Data.AdminUser);

            await AssignRolesToUser(services, userId, roleIds);
        }

        private async Task<long> CreateUser(
            IServiceProvider services,
            InitializationData.UserData userData)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var registeredUser = await userManager.FindByNameAsync(userData.Email);

            if (registeredUser is not null)
            {
                Logger.LogInformation("User {userName} already exists", userData.Email);

                return registeredUser.Id;
            }

            var user = new ApplicationUser
            {
                Email = userData.Email,
                EmailConfirmed = true,
                Enabled = true,
                Name = userData.Name,
                UserName = userData.Email,
            };

            var result = await userManager.CreateAsync(user, userData.Password);

            if (result.Succeeded is false)
            {
                throw new InvalidOperationException($"Error while creating user {userData.Email}");
            }

            Logger.LogInformation("Created user {userName}", userData.Email);

            registeredUser = await userManager.FindByNameAsync(userData.Email);

            return registeredUser.Id;
        }

        private async Task AssignRolesToUser(IServiceProvider services, long userId, params long[] roleIds)
        {
            var assignRolesToUserhandler = services.GetRequiredService<AssignRolesToUserHandler>();

            var command = new AssignRolesToUserCommand
            {
                UserId = userId,
                Roles = roleIds.Select(id => new AssignRolesToUserRole
                {
                    Id = id,
                    Selected = true,
                })
                .ToList(),
            };

            var result = await assignRolesToUserhandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                Logger.LogInformation("Assigned roles to user {userId}", userId);
            }
            else
            {
                Logger.LogWarning($"Failed to assign roles to user {userId}. {result.ErrorDescription}");
            }
        }
    }
}
