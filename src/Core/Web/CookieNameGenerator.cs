using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Nocturne.Auth.Core.Web
{
    public static class CookieNameGenerator
    {
        public static string Compute(string name, string applicationIdentifier)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(applicationIdentifier, nameof(applicationIdentifier));

            return $"{name}.{ComputeHash(applicationIdentifier)}";
        }

        private static string ComputeHash(string applicationIdentifier)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(applicationIdentifier));

            return WebEncoders.Base64UrlEncode(hash, 0, 8);
        }
    }
}
