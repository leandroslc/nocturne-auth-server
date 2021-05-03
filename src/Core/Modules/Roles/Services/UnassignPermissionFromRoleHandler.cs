using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignPermissionFromRoleHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IPermissionsRepository permissionsRepository;
        private readonly IRolePermissionsRepository rolePermissionsRepository;

        public UnassignPermissionFromRoleHandler(
            IStringLocalizer<UnassignPermissionFromRoleHandler> localizer,
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository,
            IRolePermissionsRepository rolePermissionsRepository)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.permissionsRepository = permissionsRepository;
            this.rolePermissionsRepository = rolePermissionsRepository;
        }

        public async Task<UnassignPermissionFromRoleResult> HandleAsync(UnassignPermissionFromRoleCommand command)
        {
            var role = await GetRoleAsync(command.RoleId);

            if (role is null)
            {
                return UnassignPermissionFromRoleResult.NotFound(localizer["Role not found"]);
            }

            var permission = await GetPermissionAsync(command.PermissionId);

            if (permission is null)
            {
                return UnassignPermissionFromRoleResult.NotFound(localizer["Permission not found"]);
            }

            await rolePermissionsRepository.UnassignPermissionAsync(role, permission);

            return UnassignPermissionFromRoleResult.Success();
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            return id.HasValue
                ? await rolesRepository.GetById(id.Value)
                : null;
        }

        private async Task<Permission> GetPermissionAsync(long? id)
        {
            return id.HasValue
                ? await permissionsRepository.GetById(id.Value)
                : null;
        }
    }
}
