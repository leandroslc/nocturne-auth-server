using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListUserRolesCommand : PagedCommand<ListUserRolesItem>
    {
        public string Name { get; set; }

        public long? UserId { get; set; }
    }
}
