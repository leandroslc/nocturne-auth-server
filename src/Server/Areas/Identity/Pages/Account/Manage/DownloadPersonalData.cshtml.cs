// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Configuration.Options;

namespace Nocturne.Auth.Server.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private static JsonSerializerOptions serializerOptions;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<DownloadPersonalDataModel> logger;
        private readonly string personalDataFileName;

        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger,
            IOptions<AccountOptions> accountOptions)
        {
            this.userManager = userManager;
            this.logger = logger;

            personalDataFileName = accountOptions.Value.PersonalDataFileName;
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
                SerializeData(personalData),
                "application/json",
                personalDataFileName);
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
                    property.Name.ToLowerInvariant(),
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

        private static byte[] SerializeData(Dictionary<string, object> data)
        {
            serializerOptions ??= new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            return JsonSerializer.SerializeToUtf8Bytes(data, serializerOptions);
        }
    }
}
