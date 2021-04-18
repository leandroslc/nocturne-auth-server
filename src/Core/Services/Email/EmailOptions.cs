namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailOptions
    {
        public const string Section = "Email";

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
