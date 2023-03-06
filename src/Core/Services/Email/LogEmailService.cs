// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Logging;

namespace Nocturne.Auth.Core.Services.Email
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
            logger.LogInformation(
                "Email sent.\nTo: {Email}.\nSub: {Subject}.\nMsg: {HtmlMessage}",
                email, subject, htmlMessage);

            return Task.CompletedTask;
        }

        public Task SendAsync(EmailWithTemplateCommand command)
        {
            logger.LogInformation(
                "Email sent.\nTo: {To}.\nSub: {Subject}.\nMsg: {Template}",
                command.Email, command.Subject, command.TemplateName);

            return Task.CompletedTask;
        }
    }
}
