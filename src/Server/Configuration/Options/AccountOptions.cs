namespace Nocturne.Auth.Server.Configuration.Options
{
    public class AccountOptions
    {
        public const string Section = "Account";

        public bool EnableExternalAccount { get; set; } = false;

        public bool EnableAccountDeletion { get; set; } = false;

        public bool ShowRememberLogin { get; set; } = false;

        public string PersonalDataFileName { get; set; } = "personal-data.json";
    }
}
