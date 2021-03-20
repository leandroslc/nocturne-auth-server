using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Helpers;
using Nocturne.Auth.Core.OpenIddict.Applications.Models;

namespace Nocturne.Auth.Core.OpenIddict.Applications
{
    public class CreateApplicationValidation
    {
        private readonly IStringLocalizer localizer;

        public CreateApplicationValidation(
            IStringLocalizer localizer)
        {
            this.localizer = localizer;
        }

        public IEnumerable<ValidationResult> Handle(CreateApplicationCommand command)
        {
            foreach (var result in UriHelper.Validate(
                command.RedirectUris,
                nameof(command.RedirectUris),
                InvalidUri))
            {
                yield return result;
            }

            foreach (var result in UriHelper.Validate(
                command.PostLogoutRedirectUris,
                nameof(command.PostLogoutRedirectUris),
                InvalidUri))
            {
                yield return result;
            }
        }

        private string InvalidUri(string url)
        {
            return localizer["{0} is invalid", url];
        }
    }
}
