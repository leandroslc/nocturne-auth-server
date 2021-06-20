using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);

        Task SendAsync(EmailCommandWithTemplate command);
    }
}
