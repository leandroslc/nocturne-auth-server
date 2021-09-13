// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentEmail.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions options;
        private readonly EmailSettings settings;
        private readonly IFluentEmailFactory emailFactory;

        public EmailService(
            EmailOptions options,
            EmailSettings settings,
            IFluentEmailFactory emailFactory)
        {
            this.options = options;
            this.settings = settings;
            this.emailFactory = emailFactory;
        }

        public async Task SendAsync(EmailWithTemplateCommand command)
        {
            Check.NotNull(command, nameof(command));

            Validate(command);

            var email = emailFactory.Create();

            var response = await email
                .To(command.Email)
                .Subject(command.Subject)
                .UsingTemplateFromFile(
                    GetTemplateFile(command.TemplateName),
                    CreateTemplateModel(command))
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

        private EmailTemplateModel CreateTemplateModel(EmailWithTemplateCommand command)
        {
            var model = command.TemplateModel ?? new EmailTemplateModel();

            model.ApplicationName = options.Template.ApplicationName;
            model.CompanyInfo = options.Template.CompanyInfo;
            model.CompanyLogoUrl = options.Template.CompanyLogoUrl;
            model.CompanyName = options.Template.CompanyName;

            return model;
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

        private static void Validate(EmailWithTemplateCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                throw new InvalidOperationException("The destination email cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(command.TemplateName))
            {
                throw new InvalidOperationException("The email template cannot be empty");
            }
        }
    }
}
