// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Email;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Emails
{
    public sealed class IdentityEmailService
    {
        private readonly IStringLocalizer localizer;
        private readonly IEmailService emailService;

        public IdentityEmailService(
            IStringLocalizer<IdentityEmailService> localizer,
            IEmailService emailService)
        {
            this.localizer = localizer;
            this.emailService = emailService;
        }

        public async Task SendEmailConfirmation(
            ApplicationUser user,
            string email,
            string callbackUrl)
        {
            var title = localizer["Confirm your email"];

            var command = new EmailWithTemplateCommand
            {
                Email = email,
                Subject = title,
                TemplateName = "confirm-email",
                TemplateModel = new EmailConfirmationTemplateModel
                {
                    Title = title,
                    UserName = user.FirstName,
                    Message = localizer["Please confirm your account by clicking the button below."],
                    ConfirmButtonText = localizer["Confirm"],
                    CallbackUrl = callbackUrl,
                },
            };

            await emailService.SendAsync(command);
        }

        public async Task SendResetPassword(
            ApplicationUser user,
            string email,
            string callbackUrl)
        {
            var title = localizer["Reset Password"];

            var command = new EmailWithTemplateCommand
            {
                Email = email,
                Subject = title,
                TemplateName = "reset-password",
                TemplateModel = new ResetPasswordTemplateModel
                {
                    Title = title,
                    UserName = user.FirstName,
                    Message = localizer["To reset your password click on the button below."],
                    ResetButtonText = localizer["Reset"],
                    CallbackUrl = callbackUrl,
                },
            };

            await emailService.SendAsync(command);
        }
    }
}
