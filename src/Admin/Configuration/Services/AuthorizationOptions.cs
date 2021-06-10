using System.Collections.Generic;

namespace Nocturne.Auth.Admin.Configuration.Services
{
    public sealed class AuthorizationOptions
    {
        public const string Section = "Authorization";

        public AuthorizationOptions()
        {
            Scopes = new HashSet<string>();
        }

        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public ICollection<string> Scopes { get; set; }

        public bool RequireHttps { get; set; } = true;
    }
}
