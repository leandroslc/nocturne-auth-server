using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Shared.Models;
using Nocturne.Auth.Core.Shared.Validation;
using Nocturne.Auth.Server.Areas.Identity.Emails;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IdentityEmailService emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IdentityEmailService emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string CancelUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "The name is required")]
            [MaxLength(200, ErrorMessage = "The name must have less than {1} characters")]
            [DataType(DataType.Text)]
            public string Name { get; set; }

            [Required(ErrorMessage = "The CPF is required")]
            [CPF(ErrorMessage = "The CPF is invalid")]
            public string CPF { get; set; }

            [Required(ErrorMessage = "The email is required")]
            [EmailAddress(ErrorMessage = "The email is not valid")]
            public string Email { get; set; }

            [Required(ErrorMessage = "The password is required")]
            [StringLength(100, ErrorMessage = "The password must have at least {2} and max {1} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(
            string returnUrl = null,
            string cancelUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            CancelUrl = cancelUrl ?? returnUrl;

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(
            string returnUrl = null,
            string cancelUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            CancelUrl = cancelUrl ?? returnUrl;

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid is false)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                Name = Input.Name,
                CPF = CPF.ToCPF(Input.CPF),
            };

            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded is false)
            {
                return PageWithErrors(result);
            }

            logger.LogInformation("User created a new account with password.");

            await SendEmailConfirmation(user, returnUrl);

            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
            }
            else
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }
        }

        private async Task SendEmailConfirmation(ApplicationUser user, string returnUrl)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

            await emailSender.SendEmailConfirmation(user, Input.Email, callbackUrl);
        }

        private IActionResult PageWithErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
