namespace Nocturne.Auth.Core.Services.Crypto
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);

        string Decrypt(string cypherText);
    }
}
