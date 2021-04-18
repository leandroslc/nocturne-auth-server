using System;
using System.Linq;
using System.Text;

namespace Nocturne.Auth.Core.Services.Crypto
{
    public class EncryptionOptions
    {
        public const string Section = "Encryption";

        public string Key
        {
            get => KeyBytes is null ? null : Encoding.ASCII.GetString(KeyBytes);
            set => SetKey(value);
        }

        public byte[] KeyBytes { get; private set; }

        private void SetKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Encryption key cannot be empty", nameof(key));
            }

            KeyBytes = Encoding.ASCII.GetBytes(key);

            ValidateKeySize();
        }

        private void ValidateKeySize()
        {
            // 128, 192, 256 bit keys
            var isValid = new[] {16, 24, 32}.Any(size => KeyBytes.Length == size);

            if (!isValid)
            {
                throw new ArgumentException(
                    "Invalid key size. Use a key of size 128, 192 or 256 bits",
                    nameof(KeyBytes));
            }
        }
    }
}
