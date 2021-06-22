using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(EmailWithTemplateCommand command);
    }
}
