using System;
using System.Linq;

namespace Nocturne.Auth.Authorization.Helpers
{
    internal static class UrlHelper
    {
        private const char Separator = '/';

        internal static string Combine(string baseUrl, params string[] segments)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (segments == null || segments.Length == 0)
            {
                return baseUrl;
            }

            var normalizedSegments = segments
                .Where(s => s != null)
                .Select(s => s.TrimEnd(Separator).TrimStart(Separator));

            return $"{baseUrl.TrimEnd(Separator)}/{string.Join(Separator, normalizedSegments)}";
        }

        internal static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
