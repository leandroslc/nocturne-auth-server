using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nocturne.Auth.Core.Extensions;

namespace Nocturne.Auth.Core.Helpers
{
    public static class UriHelper
    {
        public static IEnumerable<Uri> GetUris(string uris)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var value in Split(uris))
            {
                if (TryCreate(value, out var uri))
                {
                    yield return uri;
                }
            }
        }

        public static IEnumerable<ValidationResult> Validate(
            string uris,
            string memberName,
            Func<string, string> errorMessageBuilder)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var url in Split(uris))
            {
                if (!TryCreate(url, out var uri) || !uri.IsWellFormedOriginalString())
                {
                    yield return new ValidationResult(
                        errorMessageBuilder(url),
                        new[] { memberName });
                }
            }
        }

        private static bool TryCreate(string url, out Uri uri)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out uri);
        }

        private static IEnumerable<string> Split(string uris)
        {
            return uris.GetDelimitedElements();
        }
    }
}
