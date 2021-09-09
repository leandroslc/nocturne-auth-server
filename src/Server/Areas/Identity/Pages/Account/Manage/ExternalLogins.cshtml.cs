// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer localizer;

        public ExternalLoginsModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<ExternalLoginsModel> localizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.localizer = localizer;
        }

        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }

        public bool ShowRemoveButton { get; set; }

        [TempData]
        public bool ExternalLoginActionFailed { get; set; }

        [TempData]
        public bool ExternalLoginActionSucceeded { get; set; }

        [TempData]
        public string ExternalLoginStatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            CurrentLogins = await userManager.GetLoginsAsync(user);

            OtherLogins = (await signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            var result = await userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (result.Succeeded is false)
            {
                return RedirectToPageWithError(
                    localizer["The external login was not removed"]);
            }

            await signInManager.RefreshSignInAsync(user);

            return RedirectToPageWithSuccess(
                localizer["The external login was removed"]);
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");

            var properties = signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                redirectUrl,
                userManager.GetUserId(User));

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            var info = await signInManager.GetExternalLoginInfoAsync(user.Id.ToString());
            if (info == null)
            {
                throw new InvalidOperationException(
                    $"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await userManager.AddLoginAsync(user, info);
            if (result.Succeeded is false)
            {
                return RedirectToPageWithError(
                    localizer["The external login was not added. External logins can only be associated with one account"]);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToPageWithSuccess(localizer["The external login was added"]);
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return userManager.GetUserAsync(User);
        }

        private IActionResult UserNotFound()
        {
            return NotFound($"Could not find user");
        }

        private IActionResult RedirectToPageWithError(string errorMessage)
        {
            ExternalLoginActionFailed = true;
            ExternalLoginStatusMessage = errorMessage;

            return RedirectToPage();
        }

        private IActionResult RedirectToPageWithSuccess(string message)
        {
            ExternalLoginActionSucceeded = true;
            ExternalLoginStatusMessage = message;

            return RedirectToPage();
        }
    }
}
