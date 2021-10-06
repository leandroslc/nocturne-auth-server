// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
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
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityEmailService emailSender;

        public ResendEmailConfirmationModel(
            UserManager<ApplicationUser> userManager,
            IdentityEmailService emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "The email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string Email { get; set; }
        }

        public Uri ReturnUrl { get; set; }

        [TempData]
        public bool EmailSent { get; set; }

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
            if (user == null)
            {
                return PageWithStatus();
            }

            var userId = await userManager.GetUserIdAsync(user);

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);

            await emailSender.SendEmailConfirmation(user, Input.Email, new Uri(callbackUrl));

            return PageWithStatus();
        }

        private IActionResult PageWithStatus()
        {
            EmailSent = true;

            return Page();
        }
    }
}
