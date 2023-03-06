// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<ResetAuthenticatorModel> logger;

        public ResetAuthenticatorModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ResetAuthenticatorModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [TempData]
        public bool AuthenticationKeyResetSucceeded { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            await userManager.SetTwoFactorEnabledAsync(user, false);
            await userManager.ResetAuthenticatorKeyAsync(user);

            logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await signInManager.RefreshSignInAsync(user);

            AuthenticationKeyResetSucceeded = true;

            return RedirectToPage("./EnableAuthenticator");
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
