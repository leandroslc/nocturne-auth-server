using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ListUsersCommand : PagedCommand<ListUsersItem>
    {
        public string Name { get; set; }

        public string UserName { get; set; }
    }
}
