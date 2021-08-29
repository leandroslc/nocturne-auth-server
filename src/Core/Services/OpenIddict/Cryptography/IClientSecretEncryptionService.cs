namespace Nocturne.Auth.Core.Services.OpenIddict.Cryptography
{
    public interface IClientSecretEncryptionService
    {
        string Encrypt(string plainText);

        string Decrypt(string cypherText);
    }
}
