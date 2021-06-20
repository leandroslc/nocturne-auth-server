using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentEmail.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions options;
        private readonly EmailSettings settings;
        private readonly IFluentEmailFactory emailFactory;

        public EmailService(
            IOptions<EmailOptions> options,
            EmailSettings settings,
            IFluentEmailFactory emailFactory)
        {
            this.options = options.Value;
            this.settings = settings;
            this.emailFactory = emailFactory;
        }

        public async Task SendAsync(EmailCommandWithTemplate command)
        {
            var email = emailFactory.Create();

            var response = await email
                .To(command.Email)
                .Subject(command.Subject)
                .UsingTemplateFromFile(
                    GetTemplateFile(command.TemplateName),
                    command.TemplateModel)
                .SendAsync();

            if (response.Successful is false)
            {
                var errors = string.Join(", ", response.ErrorMessages);

                throw new InvalidOperationException($"Error while sending email: {errors}");
            }
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

        private string GetTemplateFile(string templateName)
        {
            Check.NotNull(templateName, nameof(templateName));

            var cultureName = CultureInfo.CurrentCulture.Name;

            if (TryGetTemplateFileByCulture(templateName, cultureName, out var filePath))
            {
                return filePath;
            }

            throw new InvalidOperationException($"No email template found for {templateName}");
        }

        private bool TryGetTemplateFileByCulture(
            string templateName,
            string cultureName,
            out string foundFile)
        {
            var fileName = settings.GetTemplateFilePath($"{templateName}.{cultureName}");

            if (File.Exists(fileName))
            {
                foundFile = fileName;

                return true;
            }

            fileName = settings.GetTemplateFilePath($"{templateName}");

            if (File.Exists(fileName))
            {
                foundFile = fileName;

                return true;
            }

            foundFile = null;

            return false;
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
