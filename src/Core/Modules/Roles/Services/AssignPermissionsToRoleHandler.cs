using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignPermissionsToRoleHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IPermissionsRepository permissionsRepository;
        private readonly IRolePermissionsRepository rolePermissionsRepository;
        private readonly CustomOpenIddictApplicationManager<Application> applicationManager;

        public AssignPermissionsToRoleHandler(
            IStringLocalizer<AssignPermissionsToRoleHandler> localizer,
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository,
            IRolePermissionsRepository rolePermissionsRepository,
            CustomOpenIddictApplicationManager<Application> applicationManager)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.permissionsRepository = permissionsRepository;
            this.rolePermissionsRepository = rolePermissionsRepository;
            this.applicationManager = applicationManager;
        }

        public async Task<AssignPermissionsToRoleCommand> CreateCommandAsync(
            long? roleId,
            string applicationId = null)
        {
            var role = await GetRoleAsync(roleId);

            var currentApplicationId = applicationId ?? role.ApplicationId;

            var command = new AssignPermissionsToRoleCommand(
                currentApplicationId,
                await GetAvailableApplicationsAsync())
            {
                RoleId = role.Id,
                Permissions = await GetAvailableApplicationPermissionsAsync(currentApplicationId),
            };

            await SetSelectedPermissionsAsync(command, role);

            return command;
        }

        public async Task<AssignPermissionsToRoleResult> HandleAsync(AssignPermissionsToRoleCommand command)
        {
            var role = await GetRoleAsync(command.RoleId);

            if (role is null)
            {
                return AssignPermissionsToRoleResult.NotFound(localizer["Role not found"]);
            }

            var selectedPermissions = command.Permissions?.Where(p => p.Selected)
                ?? Enumerable.Empty<AssignPermissionsToRolePermission>();

            var permissionsToAssign = await GetUnassignedPermissionsAsync(role, selectedPermissions);

            await rolePermissionsRepository.AssignPermissionsAsync(role, permissionsToAssign);

            return AssignPermissionsToRoleResult.Success();
        }

        public async Task<bool> RoleExistsAsync(long? id)
        {
            return id.HasValue && await rolesRepository.Exists(id.Value);
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            return id.HasValue
                ? await rolesRepository.GetById(id.Value)
                : null;
        }

        private async Task SetSelectedPermissionsAsync(
            AssignPermissionsToRoleCommand command,
            Role role)
        {
            var unassignedPermissions = await GetUnassignedPermissionsAsync(role, command.Permissions);
            var unassignedPermissionIds = unassignedPermissions.Select(p => p.Id).ToHashSet();

            foreach (var permission in command.Permissions)
            {
                if (unassignedPermissionIds.Contains(permission.Id) is false)
                {
                    permission.Selected = true;
                }
            }
        }

        private async Task<IReadOnlyCollection<Permission>> GetUnassignedPermissionsAsync(
            Role role,
            IEnumerable<AssignPermissionsToRolePermission> permissions)
        {
            var permissionIds = permissions.Select(p => p.Id);

            return await rolePermissionsRepository.GetUnassignedPermissionsAsync(role, permissionIds);
        }

        private Task<IReadOnlyCollection<AssignPermissionsToRolePermission>> GetAvailableApplicationPermissionsAsync(
            string applicationId)
        {
            return permissionsRepository.QueryByApplication(applicationId, Query);

            static IQueryable<AssignPermissionsToRolePermission> Query(IQueryable<Permission> query)
            {
                query = query.OrderBy(p => p.Name);

                return query.Select(p => new AssignPermissionsToRolePermission
                {
                    Id = p.Id,
                    Name = p.Name,
                });
            }
        }

        private async Task<IReadOnlyCollection<RoleApplication>> GetAvailableApplicationsAsync()
        {
            return await applicationManager.ListAsync(Query).ToListAsync();

            static IQueryable<RoleApplication> Query(IQueryable<Application> query)
            {
                query = query.OrderBy(p => p.DisplayName);

                return query.Select(p => new RoleApplication
                {
                    Id = p.Id,
                    Name = p.DisplayName,
                });
            }
        }
    }
}
