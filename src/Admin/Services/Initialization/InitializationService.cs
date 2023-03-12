// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Configuration.Health;
using Nocturne.Auth.Core.Modules;
using Nocturne.Auth.Core.Modules.Initialization;
using Nocturne.Auth.Core.Modules.Roles;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Services.DataProtection;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;
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

            await WaitForExternalServices(services);

            await ApplyAuthorizationMigrations(services);
            await ApplyIdentityMigrations(services);
            await ApplyDataProtectionMigrations(services);

            await CreateScopes(services);

            var adminApplicationId = await CreateAdminApplication(services);
            var adminRoleIds = await CreateAdminRole(services, adminApplicationId);
            await CreateAdminUser(services, adminRoleIds);
        }

        private async Task WaitForExternalServices(IServiceProvider services)
        {
            Logger.LogInformation("Waiting for database...");

            var databaseHealthCheck = services.GetRequiredService<DatabaseServerHealthCheck>();

            if (await HealthChecker.CheckAsync(databaseHealthCheck, delayInMilliseconds: 3000) is false)
            {
                throw new InvalidOperationException("Database connection is degraded");
            }
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

            Logger.LogInformation("Applying database migrations for {Context}", contextName);

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

            var applicationDescriptor = Data.AdminApplication;

            var registeredApplication = await applicationManager
                .FindByNameAsync(applicationDescriptor.DisplayName);

            if (registeredApplication is not null)
            {
                Logger.LogInformation("Application {DisplayName} already exists", applicationDescriptor.DisplayName);

                return await applicationManager.GetIdAsync(registeredApplication);
            }

            registeredApplication = await applicationManager.CreateAsync(applicationDescriptor);

            Logger.LogInformation(
                "Created application {DisplayName} " +
                "\"client_id: {ClientId}\" " +
                "\"client_secrect: {ClientSecret}\"",
                applicationDescriptor.DisplayName,
                applicationDescriptor.ClientId,
                applicationDescriptor.ClientSecret);

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
                    Logger.LogInformation("Scope {Name} already exists", scope.Name);

                    continue;
                }

                await scopeManager.CreateAsync(scope);

                Logger.LogInformation("Created scope {Name}", scope.Name);
            }
        }

        private async Task<IReadOnlyCollection<long>> CreateAdminRole(
            IServiceProvider services,
            string adminApplicationId)
        {
            Logger.LogInformation("Creating permissions for {AdminApplicationId}", adminApplicationId);

            return new[]
            {
                await CreateRole(
                    services,
                    Permissions.ApplicationManage,
                    "View, create and manage all applications"),
                await CreateRole(
                    services,
                    Permissions.UserRolesManage,
                    "Assign and unassign users' roles"),
            };
        }

        private async Task<long> CreateRole(
            IServiceProvider services,
            string name,
            string description)
        {
            var createRoleHandler = services.GetRequiredService<CreateRoleHandler>();

            var role = await FindRole(services, name);

            if (role is not null)
            {
                Logger.LogInformation("Role {Name} already exists", name);

                return role.Id;
            }

            var command = new CreateRoleCommand
            {
                Name = name,
                Description = description,
            };

            var result = await createRoleHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                Logger.LogInformation("Created role {Name}", name);

                return result.RoleId;
            }

            throw new InvalidOperationException(
                $"Error while creating role {name}: {result.ErrorDescription}");
        }

        private static async Task<Role> FindRole(
            IServiceProvider services,
            string name)
        {
            var repository = services.GetRequiredService<IRolesRepository>();

            var roles = await repository.Query(q => q.Where(r => r.Name == name));

            return roles.FirstOrDefault();
        }

        private async Task CreateAdminUser(
            IServiceProvider services,
            IReadOnlyCollection<long> roleIds)
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
                Logger.LogInformation("User {UserName} already exists", userData.Email);

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

            Logger.LogInformation("Created user {UserName}", userData.Email);

            registeredUser = await userManager.FindByNameAsync(userData.Email);

            return registeredUser.Id;
        }

        private async Task AssignRolesToUser(
            IServiceProvider services,
            long userId,
            IReadOnlyCollection<long> roleIds)
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
                Logger.LogInformation("Assigned roles to user {UserId}", userId);
            }
            else
            {
                Logger.LogWarning(
                    "Failed to assign roles to user {UserId}. {ErrorDescription}",
                    userId,
                    result.ErrorDescription);
            }
        }
    }
}
