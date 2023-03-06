// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ViewUserHandler
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ViewUserHandler(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ViewUserResult> HandleAsync(ViewUserCommand command)
        {
            var user = await GetUserAsync(command.Id);

            if (user is null)
            {
                return ViewUserResult.NotFound();
            }

            var userData = new ViewUserItem(user);

            return ViewUserResult.Success(userData);
        }

        private async Task<ApplicationUser> GetUserAsync(long? id)
        {
            if (id.HasValue)
            {
                var userId = id.Value.ToString(CultureInfo.InvariantCulture);

                return await userManager.FindByIdAsync(userId);
            }

            return null;
        }
    }
}
