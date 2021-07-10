namespace Nocturne.Auth.Core.Web
{
    public class WebApplicationOptions
    {
        public const string Section = "Application";

        public string ApplicationName { get; set; }

        public string CompanyName { get; set; }

        public string PrivacyPolicyUrl { get; set; }

        public bool HasPrivacyPolicyUrl => !string.IsNullOrWhiteSpace(PrivacyPolicyUrl);
    }
}
