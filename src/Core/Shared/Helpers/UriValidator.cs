using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Shared.Helpers
{
    public static class UriValidator
    {
        /// <summary>
        /// Validates the specified space or comma delimited list of uris
        /// </summary>
        /// <param name="context">An instance of <see cref="ValidationResult" /></param>
        /// <param name="uris">A space or comma delimited list of URIs</param>
        /// <param name="memberName">Member name associated with the validation</param>
        /// <returns>A collection of <see cref="ValidationResult" />s</returns>
        public static IEnumerable<ValidationResult> Validate(
            ValidationContext context,
            string uris,
            string memberName)
        {
            var invalidUris = GetInvalidUris(uris);

            var localizer = context.GetLocalizer();

            foreach (var invalidUri in invalidUris)
            {
                yield return new ValidationResult(
                    localizer["{0} is invalid", invalidUri],
                    new[] { memberName });
            }
        }

        private static IEnumerable<string> GetInvalidUris(string uris)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var uriValue in uris.GetDelimitedElements())
            {
                if (!UriHelper.TryCreate(uriValue, out var _))
                {
                    yield return uriValue;
                }
            }
        }
    }
}
