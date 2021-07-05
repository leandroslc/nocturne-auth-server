using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public SetPasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public bool SetPasswordSucceeded { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "The new password is required")]
            [StringLength(100, ErrorMessage = "The password must have at least {2} and max {1} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            var hasPassword = await userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToChangePassword();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid is false)
            {
                return Page();
            }

            var user = await GetUserAsync();
            if (user == null)
            {
                return UserNotFound();
            }

            var addPasswordResult = await userManager.AddPasswordAsync(user, Input.NewPassword);
            if (addPasswordResult.Succeeded is false)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            await signInManager.RefreshSignInAsync(user);

            SetPasswordSucceeded = true;

            return RedirectToChangePassword();
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return userManager.GetUserAsync(User);
        }

        private IActionResult UserNotFound()
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        private IActionResult RedirectToChangePassword()
        {
            return RedirectToPage("./ChangePassword");
        }
    }
}
