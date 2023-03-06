// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<Disable2faModel> logger;
        private readonly IStringLocalizer localizer;

        public Disable2faModel(
            UserManager<ApplicationUser> userManager,
            ILogger<Disable2faModel> logger,
            IStringLocalizer<Disable2faModel> localizer)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.localizer = localizer;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (await userManager.GetTwoFactorEnabledAsync(user) is false)
            {
                return NotFound("Two-factor authentication not enabled");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await userManager.SetTwoFactorEnabledAsync(user, false);
            if (disable2faResult.Succeeded is false)
            {
                return PageWithError(
                    localizer["An unexpected error occurred disabling the two-factor authenticaton"]);
            }

            logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", userManager.GetUserId(User));

            return RedirectToPage("./TwoFactorAuthentication");
        }

        private IActionResult PageWithError(string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);

            return Page();
        }
    }
}
