// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer localizer;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<IndexModel> localizer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.localizer = localizer;
        }

        public string Username { get; set; }

        [TempData]
        public bool ProfileUpdateSucceeded { get; set; }

        [TempData]
        public bool ProfileUpdateFailed { get; set; }

        [TempData]
        public string ProfileUpdateErrorMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

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
                await LoadAsync(user);

                return Page();
            }

            if (await TrySetPhoneNumber(user) is false)
            {
                return RedirectToPageWithError(localizer["Error when trying to set phone number"]);
            }

            await UpdateUserAsync(user);

            await signInManager.RefreshSignInAsync(user);

            return RedirectToPageWithSuccess();
        }

        private IActionResult RedirectToPageWithError(string errorMessage)
        {
            ProfileUpdateFailed = true;
            ProfileUpdateErrorMessage = errorMessage;

            return RedirectToPage();
        }

        private IActionResult RedirectToPageWithSuccess()
        {
            ProfileUpdateSucceeded = true;

            return RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await userManager.GetUserNameAsync(user);
            var phoneNumber = await userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
            };
        }

        private async Task UpdateUserAsync(ApplicationUser user)
        {
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            await userManager.UpdateAsync(user);
        }

        private async Task<bool> TrySetPhoneNumber(ApplicationUser user)
        {
            var phoneNumber = await userManager.GetPhoneNumberAsync(user);

            if (Input.PhoneNumber == phoneNumber)
            {
                return true;
            }

            var setPhoneResult = await userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);

            return setPhoneResult.Succeeded;
        }

        public class InputModel
        {
            [RegularExpression(@"[0-9\-\(\)\s]+", ErrorMessage = "The phone number is not valid")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "The name is required")]
            [DataType(DataType.Text)]
            [MaxLength(200, ErrorMessage = "The name must have less than {1} characters")]
            public string Name { get; set; }
        }
    }
}
