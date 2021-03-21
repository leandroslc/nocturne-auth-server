using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Commands
{
    public abstract class ManageApplicationCommand
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string ConsentType { get; set; }

        public string AllowedScopes { get; set; }

        public string RedirectUris { get; set; }

        public string PostLogoutRedirectUris { get; set; }

        public bool AllowPasswordFlow { get; set; }

        public bool AllowClientCredentialsFlow { get; set; }

        public bool AllowAuthorizationCodeFlow { get; set; }

        public bool AllowRefreshTokenFlow { get; set; }

        public bool AllowHybridFlow { get; set; }

        public bool AllowImplicitFlow { get; set; }

        public bool AllowLogoutEndpoint { get; set; }

        public List<string> AvailableScopes { get; } = new List<string>();
    }
}
