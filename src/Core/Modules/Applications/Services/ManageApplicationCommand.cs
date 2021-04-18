using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nocturne.Auth.Core.Shared.Helpers;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public abstract class ManageApplicationCommand : IValidatableObject
    {
        [Required(ErrorMessage = "The name is required")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "The type is required")]
        public string Type { get; set; } = ClientTypes.Confidential;

        [Required(ErrorMessage = "The consent type is required")]
        public string ConsentType { get; set; } = ConsentTypes.Explicit;

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

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            yield return UriValidator
                .Validate(context, RedirectUris, nameof(RedirectUris))
                .FirstOrDefault();

            yield return UriValidator
                .Validate(context, PostLogoutRedirectUris, nameof(PostLogoutRedirectUris))
                .FirstOrDefault();
        }
    }
}
