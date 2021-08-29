using Microsoft.AspNetCore.DataProtection;

namespace Nocturne.Auth.Core.Services.OpenIddict.Cryptography
{
    public class ClientSecretEncryptionService : IClientSecretEncryptionService
    {
        private const string Purpose = "client-secret";

        private readonly IDataProtector dataProtector;

        public ClientSecretEncryptionService(
            IDataProtectionProvider dataProtectionProvider)
        {
            dataProtector = dataProtectionProvider.CreateProtector(Purpose);
        }

        public string Decrypt(string cypherText)
        {
            return dataProtector.Unprotect(cypherText);
        }

        public string Encrypt(string plainText)
        {
            return dataProtector.Protect(plainText);
        }
    }
}
