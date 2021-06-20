namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailCommandWithTemplate
    {
        public EmailCommandWithTemplate(string email)
        {
            Email = email;
        }

        public string Email { get; private set; }

        public string Subject { get; set; }

        public string TemplateName { get; set; }

        public object TemplateModel { get; set; }
    }
}
