using System.Collections.Generic;
using System.Linq;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessReturn
    {
        public GetUserAccessReturn(
            IEnumerable<string> roles,
            IEnumerable<string> permissions)
        {
            Roles = roles.ToList();
            Permissions = permissions.ToList();
        }

        public IReadOnlyCollection<string> Roles { get; }

        public IReadOnlyCollection<string> Permissions { get; }
    }
}
