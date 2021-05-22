using System.Collections.Generic;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlResponse
    {
        public static UserAccessControlResponse Empty => new UserAccessControlResponse
        {
            Roles = new HashSet<string>(0),
            Permissions = new HashSet<string>(0),
        };

        public HashSet<string> Roles { get; set; }

        public HashSet<string> Permissions { get; set; }
    }
}
