// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginWithRecoveryCodeModel> logger;
        private readonly IStringLocalizer localizer;

        public LoginWithRecoveryCodeModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginWithRecoveryCodeModel> logger,
            IStringLocalizer<LoginWithRecoveryCodeModel> localizer)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Uri ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(Uri returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Uri returnUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"), UriKind.RelativeOrAbsolute);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = NormalizeCode(Input.RecoveryCode);

            var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);

                return LocalRedirect(ReturnUrl.OriginalString);
            }

            if (result.IsLockedOut)
            {
                logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);

                return RedirectToPage("./Lockout");
            }

            logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);

            ModelState.AddModelError(string.Empty, localizer["Invalid recovery code entered"]);

            return Page();
        }

        private static string NormalizeCode(string code)
        {
            return code.Replace(" ", string.Empty, StringComparison.Ordinal);
        }

        public class InputModel
        {
            [BindProperty]
            [Required(ErrorMessage = "The code is required")]
            [DataType(DataType.Text)]
            public string RecoveryCode { get; set; }
        }
    }
}
