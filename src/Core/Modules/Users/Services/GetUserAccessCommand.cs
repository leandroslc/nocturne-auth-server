using System.Security.Claims;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessCommand
    {
        public string ClientId { get; set; }

        public ClaimsPrincipal User { get; set; }
    }
}
