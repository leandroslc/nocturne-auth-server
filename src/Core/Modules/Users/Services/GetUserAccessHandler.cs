// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CustomOpenIddictApplicationManager<Application> applicationManager;
        private readonly IUserRolesRepository userRolesRepository;

        public GetUserAccessHandler(
            IStringLocalizer<GetUserAccessHandler> localizer,
            UserManager<ApplicationUser> userManager,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IUserRolesRepository userRolesRepository)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.applicationManager = applicationManager;
            this.userRolesRepository = userRolesRepository;
        }

        public async Task<GetUserAccessResult> HandleAsync(GetUserAccessCommand command)
        {
            var application = await applicationManager.FindByClientIdAsync(command.ClientId);

            if (application is null)
            {
                return GetUserAccessResult.NotFound(
                    localizer["Invalid client id"]);
            }

            var user = await userManager.GetUserAsync(command.User);

            if (user is null)
            {
                return GetUserAccessResult.NotFound(
                    localizer["Invalid user"]);
            }

            var permissions = await GetUserPermissionsAsync(application, user);

            return GetUserAccessResult.Success(permissions);
        }

        private async Task<GetUserAccessReturn> GetUserPermissionsAsync(
            Application application,
            ApplicationUser user)
        {
            var query = userRolesRepository.QueryRolePermissionsByUser(user.Id);

            var permissions = await (
                from rolePermission in query
                where rolePermission.Role.ApplicationId == application.Id
                select new
                {
                    RoleName = rolePermission.Role.Name,
                    PermissionName = rolePermission.Permission.Name,
                })
                .ToListAsync();

            return new GetUserAccessReturn(
                roles: permissions.GroupBy(p => p.RoleName).Select(g => g.Key),
                permissions: permissions.Select(p => p.PermissionName)
            );
        }
    }
}
