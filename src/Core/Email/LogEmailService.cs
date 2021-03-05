using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Nocturne.Auth.Core.Email
{
    public class LogEmailService : IEmailService
    {
        private readonly ILogger<LogEmailService> logger;

        public LogEmailService(ILogger<LogEmailService> logger)
        {
            this.logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            logger.LogInformation($"Email sent.\nTo: {email}.\nSub: {subject}.\nMsg: {htmlMessage}");

            return Task.CompletedTask;
        }
    }
}
