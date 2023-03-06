// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Areas.Identity.Emails;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityEmailService emailSender;

        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager,
            IdentityEmailService emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Uri ReturnUrl { get; set; }

        public void OnGet(Uri returnUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);
        }

        public async Task<IActionResult> OnPostAsync(Uri returnUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);

            if (ModelState.IsValid is false)
            {
                return Page();
            }

            var user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null || await userManager.IsEmailConfirmedAsync(user) is false)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return ConfirmationPage();
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await emailSender.SendResetPassword(user, Input.Email, new Uri(callbackUrl));

            return ConfirmationPage();
        }

        private IActionResult ConfirmationPage()
        {
            return RedirectToPage("./ForgotPasswordConfirmation", new { returnUrl = ReturnUrl });
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string Email { get; set; }
        }
    }
}
