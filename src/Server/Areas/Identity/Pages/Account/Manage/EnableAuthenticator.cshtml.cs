// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Configuration.Options;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class EnableAuthenticatorModel : PageModel
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<EnableAuthenticatorModel> logger;
        private readonly UrlEncoder urlEncoder;
        private readonly IStringLocalizer localizer;
        private readonly string applicationName;

        public EnableAuthenticatorModel(
            UserManager<ApplicationUser> userManager,
            ILogger<EnableAuthenticatorModel> logger,
            UrlEncoder urlEncoder,
            IStringLocalizer<EnableAuthenticatorModel> localizer,
            IOptions<ServerApplicationOptions> applicationOptions)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.urlEncoder = urlEncoder;
            this.localizer = localizer;

            applicationName = applicationOptions.Value.ApplicationName;
        }

        public string SharedKey { get; set; }

        public Uri AuthenticatorUri { get; set; }

        [TempData]
        [SuppressMessage("Usage", "CA2227",
            Justification = "TempData requires it public and not read-only")]
        public ICollection<string> RecoveryCodes { get; set; }

        [TempData]
        public bool EnableAuthenticatorSucceeded { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadSharedKeyAndQrCodeUriAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid is false)
            {
                return await ReloadPage(user);
            }

            var verificationCode = NormalizeCode(Input.Code);

            var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                userManager.Options.Tokens.AuthenticatorTokenProvider,
                verificationCode);

            if (is2faTokenValid is false)
            {
                ModelState.AddModelError(string.Empty, localizer["Verification code is invalid"]);

                return await ReloadPage(user);
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);

            var userId = await userManager.GetUserIdAsync(user);

            logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

            EnableAuthenticatorSucceeded = true;

            if (await userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

                RecoveryCodes = recoveryCodes.ToArray();

                return RedirectToPage("./ShowRecoveryCodes");
            }
            else
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }
        }

        private async Task<IActionResult> ReloadPage(ApplicationUser user)
        {
            await LoadSharedKeyAndQrCodeUriAsync(user);

            return Page();
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);

                unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            }

            SharedKey = FormatKey(unformattedKey);

            var email = await userManager.GetEmailAsync(user);

            AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);
        }

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;

            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey[currentPosition..(currentPosition + 4)]).Append(' ');
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey[currentPosition..]);
            }

            return result.ToString().ToLowerInvariant();
        }

        private Uri GenerateQrCodeUri(string email, string unformattedKey)
        {
            var authenticatorUrl = string.Format(
                CultureInfo.InvariantCulture,
                AuthenticatorUriFormat,
                urlEncoder.Encode(applicationName),
                urlEncoder.Encode(email),
                unformattedKey);

            return new Uri(authenticatorUrl);
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
            public string Code { get; set; }
        }
    }
}
