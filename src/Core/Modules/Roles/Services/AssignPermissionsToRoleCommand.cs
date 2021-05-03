using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignPermissionsToRoleCommand
    {
        public long? RoleId { get; set; }

        public IReadOnlyCollection<AssignPermissionsToRolePermission> Permissions { get; set; }
    }
}
