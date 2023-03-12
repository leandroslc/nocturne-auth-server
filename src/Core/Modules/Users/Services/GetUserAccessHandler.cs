// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRolesRepository userRolesRepository;

        public GetUserAccessHandler(
            IStringLocalizer<GetUserAccessHandler> localizer,
            UserManager<ApplicationUser> userManager,
            IUserRolesRepository userRolesRepository)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.userRolesRepository = userRolesRepository;
        }

        public async Task<GetUserAccessResult> HandleAsync(GetUserAccessCommand command)
        {
            var user = await userManager.GetUserAsync(command.User);

            if (user is null)
            {
                return GetUserAccessResult.NotFound(
                    localizer["Invalid user"]);
            }

            var permissions = await GetUserPermissionsAsync(user);

            return GetUserAccessResult.Success(permissions);
        }

        private async Task<GetUserAccessReturn> GetUserPermissionsAsync(
            ApplicationUser user)
        {
            var query = userRolesRepository.QueryByUser(user.Id);

            var roles = await (
                from role in query
                select role.Name)
                .ToListAsync();

            return new GetUserAccessReturn(roles: roles);
        }
    }
}
