namespace Nocturne.Auth.Server.Configuration.Options
{
    public class OpenIdServerOptions
    {
        public const string Section = "OpenIdServer";

        public bool UseDevelopmentCertificates { get; set; }

        public string EncryptionCertificateThumbprint { get; set; }

        public string SigningCertificateThumbprint { get; set; }

        public bool DisableTransportSecurityRequirement { get; set; }
    }
}
