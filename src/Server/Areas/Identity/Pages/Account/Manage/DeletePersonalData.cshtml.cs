// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Configuration.Options;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IStringLocalizer localizer;
        private readonly bool enableAccountDeletion;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IStringLocalizer<DeletePersonalDataModel> localizer,
            IOptions<AccountOptions> accountOptions)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.localizer = localizer;

            enableAccountDeletion = accountOptions.Value.EnableAccountDeletion;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (enableAccountDeletion is false)
            {
                return NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            RequirePassword = await userManager.HasPasswordAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (enableAccountDeletion is false)
            {
                return NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            RequirePassword = await userManager.HasPasswordAsync(user);

            if (RequirePassword && await PasswordIsIncorrect(user))
            {
                return PageWithError(localizer["Incorrect password"]);
            }

            var result = await userManager.DeleteAsync(user);
            var userId = await userManager.GetUserIdAsync(user);
            if (result.Succeeded is false)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            // TODO: Sign out using the authorization logout endpoint
            await signInManager.SignOutAsync();

            logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }

        private async Task<bool> PasswordIsIncorrect(ApplicationUser user)
        {
            return await userManager.CheckPasswordAsync(user, Input.Password) is false;
        }

        private IActionResult PageWithError(string errorMessage)
        {
            ModelState.AddModelError(string.Empty, errorMessage);

            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
