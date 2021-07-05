using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private const string PersonalDataFileName = "PersonalData.json";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<DownloadPersonalDataModel> logger;

        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            logger.LogInformation(
                "User with ID '{UserId}' asked for their personal data.",
                userManager.GetUserId(User));

            var personalData = new Dictionary<string, object>();

            AddUserPersonalData(personalData, user);

            await AdUserLoginProviders(personalData, user);

            return File(
                JsonSerializer.SerializeToUtf8Bytes(personalData),
                "application/json",
                PersonalDataFileName);
        }

        private static void AddUserPersonalData(
            Dictionary<string, object> personalData,
            ApplicationUser user)
        {
            // Only include personal data for download
            var personalDataProps = typeof(ApplicationUser)
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var property in personalDataProps)
            {
                var value = property.GetValue(user);

                personalData.Add(
                    property.Name,
                    property.PropertyType.IsPrimitive ? value : value?.ToString());
            }
        }

        private async Task AdUserLoginProviders(
            Dictionary<string, object> personalData,
            ApplicationUser user)
        {
            var logins = await userManager.GetLoginsAsync(user);

            foreach (var login in logins)
            {
                personalData.Add($"{login.LoginProvider} external login provider key", login.ProviderKey);
            }
        }
    }
}
