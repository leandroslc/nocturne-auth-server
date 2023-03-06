// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Configuration.Options;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly IStringLocalizer localizer;
        private readonly AccountOptions accountOptions;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            IStringLocalizer<LoginModel> localizer,
            IOptions<AccountOptions> accountOptions)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.localizer = localizer;
            this.accountOptions = accountOptions.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; private set; }

        public Uri ReturnUrl { get; set; }

        public bool EnableExternalAccount => accountOptions.EnableExternalAccount;

        public bool ShowRememberLogin => accountOptions.ShowRememberLogin;

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(Uri returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            await SetExternalLogins();
        }

        public async Task<IActionResult> OnPostAsync(Uri returnUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);

            await SetExternalLogins();

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    Input.Email,
                    Input.Password,
                    Input.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");

                    return LocalRedirect(ReturnUrl.OriginalString);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage(
                        "./LoginWith2fa",
                        new { returnUrl = ReturnUrl.PathAndQuery, rememberMe = Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");

                    return RedirectToPage("./Lockout", new { returnUrl = Request.GetEncodedPathAndQuery() });
                }

                // If we got this far, something failed
                ModelState.AddModelError(
                    string.Empty,
                    localizer["Invalid login attempt"]);
            }

            return Page();
        }

        private async Task SetExternalLogins()
        {
            if (EnableExternalAccount)
            {
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string Email { get; set; }

            [Required(ErrorMessage = "The password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}
