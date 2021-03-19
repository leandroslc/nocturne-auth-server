namespace Nocturne.Auth.Core.Crypto
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);

        string Decrypt(string cypherText);
    }
}
