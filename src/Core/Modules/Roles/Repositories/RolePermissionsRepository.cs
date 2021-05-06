using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Modules.Permissions;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public class RolePermissionsRepository : IRolePermissionsRepository
    {
        private readonly AuthorizationDbContext context;

        public RolePermissionsRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public async Task AssignPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var rolePermissions = permissions.Select(p => new RolePermission(role, p));

            context.AddRange(rolePermissions);

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Permission>> GetUnassignedPermissionsAsync(Role role, IEnumerable<long> ids)
        {
            return await
                (from p in context.Set<Permission>()
                 join rp in context.Set<RolePermission>() on p.Id equals rp.PermissionId into j
                 from r in j.DefaultIfEmpty()
                 where ids.Contains(p.Id) && r == null
                 select p)
                 .ToListAsync();
        }

        public async Task UnassignPermissionAsync(Role role, Permission permission)
        {
            var rolePermission = await context
                .Set<RolePermission>()
                .FirstOrDefaultAsync(p => p.RoleId == role.Id && p.PermissionId == permission.Id);

            if (rolePermission is null)
            {
                return;
            }

            context.Remove(rolePermission);

            await context.SaveChangesAsync();
        }
    }
}
