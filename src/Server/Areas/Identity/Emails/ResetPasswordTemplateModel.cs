using Nocturne.Auth.Core.Services.Email;

namespace Nocturne.Auth.Server.Areas.Identity.Emails
{
    public class ResetPasswordTemplateModel : EmailTemplateModel
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public string CallbackUrl { get; set; }

        public string ResetButtonText { get; set; }
    }
}
