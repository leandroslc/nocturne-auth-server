// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class GenerateRecoveryCodesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<GenerateRecoveryCodesModel> logger;

        public GenerateRecoveryCodesModel(
            UserManager<ApplicationUser> userManager,
            ILogger<GenerateRecoveryCodesModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [TempData]
        [SuppressMessage("Usage", "CA2227",
            Justification = "TempData requires it public and not read-only")]
        public ICollection<string> RecoveryCodes { get; set; }

        [TempData]
        public bool GenerateRecoveryCodesSucceeded { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);
            if (isTwoFactorEnabled is false)
            {
                var userId = await userManager.GetUserIdAsync(user);

                return TwoFactorNotEnabled(userId);
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

            var userId = await userManager.GetUserIdAsync(user);
            var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            if (isTwoFactorEnabled is false)
            {
                return TwoFactorNotEnabled(userId);
            }

            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            RecoveryCodes = recoveryCodes.ToArray();

            logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);

            GenerateRecoveryCodesSucceeded = true;

            return RedirectToPage("./ShowRecoveryCodes");
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return userManager.GetUserAsync(User);
        }

        private IActionResult UserNotFound()
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        private IActionResult TwoFactorNotEnabled(string userId)
        {
            return BadRequest($"Cannot generate recovery codes for user with ID '{userId}' because they do not have 2FA enabled.");
        }
    }
}
