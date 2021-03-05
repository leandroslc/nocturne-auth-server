using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
