// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignRoleFromUserHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IUserRolesRepository userRolesRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UnassignRoleFromUserHandler(
            IStringLocalizer<UnassignRoleFromUserHandler> localizer,
            IRolesRepository rolesRepository,
            IUserRolesRepository userRolesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.userRolesRepository = userRolesRepository;
            this.userManager = userManager;
        }

        public async Task<UnassignRoleFromUserResult> HandleAsync(UnassignRoleFromUserCommand command)
        {
            var user = await GetUserAsync(command.UserId);

            if (user is null)
            {
                return UnassignRoleFromUserResult.NotFound(localizer["User not found"]);
            }

            var role = await GetRoleAsync(command.RoleId);

            if (role is null)
            {
                return UnassignRoleFromUserResult.NotFound(localizer["Role not found"]);
            }

            await userRolesRepository.UnassignRoleAsync(user, role);

            return UnassignRoleFromUserResult.Success();
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            return id.HasValue
                ? await rolesRepository.GetById(id.Value)
                : null;
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
