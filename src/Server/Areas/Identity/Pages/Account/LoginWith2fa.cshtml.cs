// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginWith2faModel> logger;
        private readonly IStringLocalizer localizer;

        public LoginWith2faModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginWith2faModel> logger,
            IStringLocalizer<LoginWith2faModel> localizer)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public Uri ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, Uri returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return BadRequest($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, Uri returnUrl = null)
        {
            if (ModelState.IsValid is false)
            {
                return Page();
            }

            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = NormalizeCode(Input.TwoFactorCode);

            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(
                authenticatorCode,
                rememberMe,
                Input.RememberMachine);

            if (result.Succeeded)
            {
                logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);

                return LocalRedirect(ReturnUrl.OriginalString);
            }

            if (result.IsLockedOut)
            {
                logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);

                return RedirectToPage("./Lockout");
            }

            logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);

            ModelState.AddModelError(string.Empty, localizer["Invalid authenticator code"]);

            return Page();
        }

        private static string NormalizeCode(string code)
        {
            return code
                .Replace(" ", string.Empty, StringComparison.Ordinal)
                .Replace("-", string.Empty, StringComparison.Ordinal);
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The code is required")]
            [StringLength(7, ErrorMessage = "The code must have at least {2} and max {1} characters", MinimumLength = 6)]
            [DataType(DataType.Text)]
            public string TwoFactorCode { get; set; }

            public bool RememberMachine { get; set; }
        }
    }
}
