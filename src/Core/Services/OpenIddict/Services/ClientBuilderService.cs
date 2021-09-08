using System.Security.Cryptography;
using Base62;

namespace Nocturne.Auth.Core.Services.OpenIddict.Services
{
    public class ClientBuilderService : IClientBuilderService
    {
        public string GenerateClientId()
        {
            var bytes = GenerateRandomBytes(16);

            return ToBase62String(bytes);
        }

        public string GenerateClientSecret()
        {
            var bytes = GenerateRandomBytes(32);

            return ToBase62String(bytes);
        }

        private static string ToBase62String(byte[] bytes)
        {
            return bytes.ToBase62();
        }

        private static byte[] GenerateRandomBytes(int size)
        {
            using var rng = RandomNumberGenerator.Create();

            var bytes = new byte[size];

            rng.GetBytes(bytes);

            return bytes;
        }
    }
}
