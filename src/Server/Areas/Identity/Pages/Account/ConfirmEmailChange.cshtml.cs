// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer localizer;

        public ConfirmEmailChangeModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<ConfirmEmailChangeModel> localizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.localizer = localizer;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ChangeEmailAsync(user, email, code);
            if (result.Succeeded is false)
            {
                return PageWithError(localizer["Error changing the email."]);
            }

            // For now, the email is used as the username, so the username should also be updated.
            var setUserNameResult = await userManager.SetUserNameAsync(user, email);
            if (setUserNameResult.Succeeded is false)
            {
                return PageWithError(localizer["Error changing user name."]);
            }

            await signInManager.RefreshSignInAsync(user);

            return PageWithSuccess();
        }

        private IActionResult PageWithError(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;

            return Page();
        }

        private IActionResult PageWithSuccess()
        {
            Success = true;

            return Page();
        }
    }
}
