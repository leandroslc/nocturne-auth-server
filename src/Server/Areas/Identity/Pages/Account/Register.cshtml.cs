// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication;
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
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IdentityEmailService emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IdentityEmailService emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Uri ReturnUrl { get; set; }

        public Uri CancelUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; private set; }

        public async Task OnGetAsync(
            Uri returnUrl = null,
            Uri cancelUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);
            CancelUrl = cancelUrl ?? ReturnUrl;

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(
            Uri returnUrl = null,
            Uri cancelUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);
            CancelUrl = cancelUrl ?? ReturnUrl;

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid is false)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Name = Input.Name,
            };

            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded is false)
            {
                return PageWithErrors(result);
            }

            logger.LogInformation("User created a new account with password.");

            await SendEmailConfirmation(user, ReturnUrl);

            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = ReturnUrl });
            }
            else
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(ReturnUrl.OriginalString);
            }
        }

        private async Task SendEmailConfirmation(ApplicationUser user, Uri returnUrl)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

            await emailSender.SendEmailConfirmation(user, Input.Email, new Uri(callbackUrl));
        }

        private IActionResult PageWithErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The name is required")]
            [MaxLength(200, ErrorMessage = "The name must have less than {1} characters")]
            [DataType(DataType.Text)]
            public string Name { get; set; }

            [Required(ErrorMessage = "The email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string Email { get; set; }

            [Required(ErrorMessage = "The password is required")]
            [StringLength(100, ErrorMessage = "The password must have at least {2} and max {1} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
