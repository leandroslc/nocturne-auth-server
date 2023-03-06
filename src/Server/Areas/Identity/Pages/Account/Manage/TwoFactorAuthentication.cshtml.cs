// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public TwoFactorAuthenticationModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public bool HasAuthenticator { get; set; }

        public int RecoveryCodesLeft { get; set; }

        [BindProperty]
        public bool Is2faEnabled { get; set; }

        public bool IsMachineRemembered { get; set; }

        [TempData]
        public bool ForgetBrowserSucceeded { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            HasAuthenticator = await userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await userManager.CountRecoveryCodesAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            await signInManager.ForgetTwoFactorClientAsync();

            ForgetBrowserSucceeded = true;

            return RedirectToPage();
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return userManager.GetUserAsync(User);
        }

        private IActionResult UserNotFound()
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }
    }
}
