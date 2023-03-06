// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<ChangePasswordModel> logger;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public bool UpdatePasswordSucceeded { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var hasPassword = await userManager.HasPasswordAsync(user);
            if (hasPassword is false)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid is false)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(
                user,
                Input.OldPassword,
                Input.NewPassword);

            if (changePasswordResult.Succeeded is false)
            {
                return PageWithErrors(changePasswordResult.Errors);
            }

            await signInManager.RefreshSignInAsync(user);

            logger.LogInformation("User changed their password successfully.");

            return RedirectToPageWithSuccess();
        }

        private IActionResult RedirectToPageWithSuccess()
        {
            UpdatePasswordSucceeded = true;

            return RedirectToPage();
        }

        private IActionResult PageWithErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "The password is required")]
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "The new password is required")]
            [StringLength(100, ErrorMessage = "The password must have at least {2} and max {1} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
