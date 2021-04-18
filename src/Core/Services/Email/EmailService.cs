using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions options;

        public EmailService(IOptions<EmailOptions> options)
        {
            this.options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Subject = subject,
                Body = new TextPart("html")
                {
                    Text = htmlMessage,
                },
            };

            message.From.Add(new MailboxAddress(options.SenderName, options.SenderEmail));
            message.To.Add(new MailboxAddress(email, email));

            using var client = new SmtpClient();

            await client.ConnectAsync(options.Host, options.Port, GetSSLOptions());

            if (CanUseAuthentication())
            {
                await client.AuthenticateAsync(options.Login, options.Password);
            }

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }

        private bool CanUseAuthentication()
        {
            return string.IsNullOrWhiteSpace(options.Password) == false;
        }

        private SecureSocketOptions GetSSLOptions()
        {
            return options.UseSSL
                ? SecureSocketOptions.Auto
                : SecureSocketOptions.None;
        }
    }
}
