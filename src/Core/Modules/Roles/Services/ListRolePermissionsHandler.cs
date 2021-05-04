using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListRolePermissionsHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IPermissionsRepository permissionsRepository;

        public ListRolePermissionsHandler(
            IStringLocalizer<ListRolePermissionsHandler> localizer,
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.permissionsRepository = permissionsRepository;
        }

        public async Task<ListRolePermissionsResult> HandleAsync(
            ListRolePermissionsCommand command)
        {
            var role = await GetRoleAsync(command.RoleId);

            if (role is null)
            {
                return ListRolePermissionsResult.NotFound(localizer["Role not found"]);
            }

            var permissions = await permissionsRepository.QueryByRole(
                role.Id, GetPermissionsQuery);

            return ListRolePermissionsResult.Success(permissions);
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            return id.HasValue
                ? await rolesRepository.GetById(id.Value)
                : null;
        }

        private static IQueryable<ListRolePermissionsItem> GetPermissionsQuery(
            IQueryable<Permission> query)
        {
            query = query.OrderBy(p => p.Name);

            return query.Select(p => new ListRolePermissionsItem
            {
                Id = p.Id,
                Name = p.Name,
                ApplicationId = p.Application.Id,
                ApplicationName = p.Application.DisplayName,
            });
        }
    }
}
