namespace Nocturne.Auth.Configuration.Models
{
    public class ApplicationDataProtectionOptions
    {
        public const string Section = "DataProtection";

        public string EncryptionCertificateThumbprint { get; set; }
    }
}
