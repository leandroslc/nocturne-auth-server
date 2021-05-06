using System.Collections.Generic;
using Nocturne.Auth.Core.Modules.Roles.Services;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class RolePermissionsViewModel
    {
        public RolePermissionsViewModel(
            long roleId,
            IReadOnlyCollection<ListRolePermissionsItem> permissions)
        {
            RoleId = roleId;
            Permissions = permissions;
        }

        public long RoleId { get; private set; }

        public IReadOnlyCollection<ListRolePermissionsItem> Permissions { get; private set; }
    }
}
