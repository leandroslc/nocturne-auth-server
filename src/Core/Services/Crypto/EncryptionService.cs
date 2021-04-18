using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Nocturne.Auth.Core.Services.Crypto
{
    public class EncryptionService : IEncryptionService
    {
        private const int VectorSizeBytesLength = sizeof(int);

        private readonly EncryptionOptions options;

        public EncryptionService(
            IOptions<EncryptionOptions> options)
        {
            this.options = options.Value;
        }

        public string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.Unicode.GetBytes(plainText);

            using var aes = Aes.Create();

            var iv = aes.IV;

            using var stream = new MemoryStream();

            stream.Write(ConvertIntToBytes(iv.Length), 0, VectorSizeBytesLength);
            stream.Write(iv, 0, iv.Length);

            using var cryptoStream = new CryptoStream(
                stream,
                aes.CreateEncryptor(options.KeyBytes, iv),
                CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(stream.ToArray());
        }

        public string Decrypt(string cypherText)
        {
            var cypherTextBytes = Convert.FromBase64String(cypherText);

            using var aes = Aes.Create();

            using var stream = new MemoryStream(cypherTextBytes);

            var vectorLengthBytes = new byte[VectorSizeBytesLength];
            stream.Read(vectorLengthBytes, 0, VectorSizeBytesLength);

            var vectorLength = ConvertBytesToInt(vectorLengthBytes);

            var iv = new byte[vectorLength];
            stream.Read(iv, 0, vectorLength);

            using var cryptoStream = new CryptoStream(
                stream,
                aes.CreateDecryptor(options.KeyBytes, iv),
                CryptoStreamMode.Read);

            var cypherLength = Math.Abs(VectorSizeBytesLength + vectorLength - cypherTextBytes.Length);

            var plaintTextBytes = new byte[cypherLength];

            using var plainStream = new MemoryStream();

            cryptoStream.CopyTo(plainStream);

            return Encoding.Unicode.GetString(plainStream.ToArray());
        }

        private static byte[] ConvertIntToBytes(int value)
        {
            var bytes = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        private static int ConvertBytesToInt(byte[] value)
        {
            var bytes = value;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToInt32(bytes);
        }
    }
}
