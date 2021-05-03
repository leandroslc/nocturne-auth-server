using System.Collections.Generic;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Permissions;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IRolePermissionsRepository
    {
        public Task AssignPermissionsAsync(Role role, IEnumerable<Permission> permissions);

        public Task<IReadOnlyCollection<Permission>> GetUnassignedPermissionsAsync(Role role, IEnumerable<long> ids);

        public Task UnassignPermissionAsync(Role role, Permission permission);
    }
}
