using System;
using System.Collections.Generic;
using Nocturne.Auth.Core.Extensions;

namespace Nocturne.Auth.Core.Helpers
{
    /// <summary>
    /// A helper class to get and validate URIs
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Gets a set of <see cref="Uri" />s from the specified value.
        /// Invalid uris are ignored
        /// </summary>
        /// <param name="uris">A space or comma delimited list of URIs</param>
        /// <returns>A collection of valid <see cref="Uri" />s</returns>
        public static IEnumerable<Uri> GetUris(string uris)
        {
            if (uris == null)
            {
                yield break;
            }

            foreach (var value in uris.GetDelimitedElements())
            {
                if (TryCreate(value, out var uri))
                {
                    yield return uri;
                }
            }
        }

        /// <summary>
        /// Creates a new absolute <see cref="Uri" /> using the specified value,
        /// also validating if the URI is well-formed
        /// </summary>
        /// <param name="value">The value representing the <see cref="Uri" /></param>
        /// <param name="uri">The constructed <see cref="Uri" /></param>
        /// <returns>true if it was successfully created or false otherwise</returns>
        public static bool TryCreate(string value, out Uri uri)
        {
            var created = Uri.TryCreate(value, UriKind.Absolute, out uri);

            return created && uri.IsWellFormedOriginalString();
        }
    }
}
