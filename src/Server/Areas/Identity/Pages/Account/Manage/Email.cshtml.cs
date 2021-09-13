// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Areas.Identity.Emails;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityEmailService emailSender;
        private readonly IStringLocalizer localizer;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            IdentityEmailService emailSender,
            IStringLocalizer<EmailModel> localizer)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.localizer = localizer;
        }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string ManageEmailStatusMessage { get; set; }

        [TempData]
        public bool ManageEmailStatusIsSuccess { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "The new email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string NewEmail { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            if (ModelState.IsValid is false)
            {
                await LoadAsync(user);

                return Page();
            }

            var email = await userManager.GetEmailAsync(user);

            if (Input.NewEmail == email)
            {
                return PageWithSuccess(
                    localizer["Your email is unchanged"]);
            }

            var userId = await userManager.GetUserIdAsync(user);

            var code = Encode(
                await userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail));

            await SendConfirmationEmail(
                user,
                Input.NewEmail,
                "/Account/ConfirmEmailChange",
                new { userId, email = Input.NewEmail, code });

            return PageWithSuccess(
                localizer["Confirmation link to change email sent. Please check your email"]);
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            if (ModelState.IsValid is false)
            {
                await LoadAsync(user);

                return Page();
            }

            var userId = await userManager.GetUserIdAsync(user);
            var email = await userManager.GetEmailAsync(user);

            var code = Encode(
                await userManager.GenerateEmailConfirmationTokenAsync(user));

            await SendConfirmationEmail(
                user,
                email,
                "/Account/ConfirmEmail",
                new { area = "Identity", userId, code });

            return PageWithSuccess(localizer["Verification email sent. Please check your email"]);
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return userManager.GetUserAsync(User);
        }

        private IActionResult UserNotFound()
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await userManager.GetEmailAsync(user);

            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        }

        private async Task SendConfirmationEmail(
            ApplicationUser user,
            string email,
            string page,
            object values)
        {
            var callbackUrl = Url.Page(
                page,
                pageHandler: null,
                values: values,
                protocol: Request.Scheme);

            await emailSender.SendEmailConfirmation(user, email, callbackUrl);
        }

        private IActionResult PageWithSuccess(string message)
        {
            ManageEmailStatusMessage = message;
            ManageEmailStatusIsSuccess = true;

            return RedirectToPage();
        }

        private static string Encode(string token)
        {
            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        }
    }
}
