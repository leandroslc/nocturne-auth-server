// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IUserRolesRepository userRolesRepository;
        private readonly CustomOpenIddictApplicationManager<Application> applicationManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AssignRolesToUserHandler(
            IStringLocalizer<AssignRolesToUserHandler> localizer,
            IRolesRepository rolesRepository,
            IUserRolesRepository userRolesRepository,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            UserManager<ApplicationUser> userManager)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.userRolesRepository = userRolesRepository;
            this.applicationManager = applicationManager;
            this.userManager = userManager;
        }

        public async Task<AssignRolesToUserCommand> CreateCommandAsync(
            long? userId,
            string applicationId = null)
        {
            var user = await GetUserAsync(userId);

            var availableRoles = applicationId is null
                ? null
                : await GetAvailableApplicationRolesAsync(applicationId);

            var command = new AssignRolesToUserCommand(
                applicationId,
                await GetAvailableApplicationsAsync())
            {
                UserId = user.Id,
                Roles = availableRoles,
            };

            if (availableRoles is not null)
            {
                await SetSelectedRolesAsync(command, user);
            }

            return command;
        }

        public async Task<AssignRolesToUserResult> HandleAsync(AssignRolesToUserCommand command)
        {
            var user = await GetUserAsync(command.UserId);

            if (user is null)
            {
                return AssignRolesToUserResult.NotFound(localizer["User not found"]);
            }

            var selectedRoles = command.Roles?.Where(role => role.Selected)
                ?? Enumerable.Empty<AssignRolesToUserRole>();

            var selectedRoleIds = selectedRoles.Select(role => role.Id).ToHashSet();

            var unassignedRoles = await GetUnassignedRolesAsync(user, command.ApplicationId);

            var rolesToAssign = unassignedRoles
                .Where(role => selectedRoleIds.Contains(role.Id));

            await userRolesRepository.AssignRolesAsync(user, rolesToAssign);

            return AssignRolesToUserResult.Success();
        }

        public async Task<bool> UserExistsAsync(long? id)
        {
            if (id.HasValue)
            {
                var userId = id.Value.ToString(CultureInfo.InvariantCulture);

                return await userManager.FindByIdAsync(userId) is not null;
            }

            return false;
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

        private async Task SetSelectedRolesAsync(
            AssignRolesToUserCommand command,
            ApplicationUser user)
        {
            var unassignedRoles = await GetUnassignedRolesAsync(user, command.ApplicationId);
            var unassignedRoleIds = unassignedRoles.Select(p => p.Id).ToHashSet();

            foreach (var role in command.Roles)
            {
                if (unassignedRoleIds.Contains(role.Id) is false)
                {
                    role.Selected = true;
                }
            }
        }

        private async Task<IReadOnlyCollection<Role>> GetUnassignedRolesAsync(
            ApplicationUser user,
            string applicationId)
        {
            return await userRolesRepository.GetUnassignedRolesAsync(user, applicationId);
        }

        private Task<IReadOnlyCollection<AssignRolesToUserRole>> GetAvailableApplicationRolesAsync(
            string applicationId)
        {
            return rolesRepository.QueryByApplication(applicationId, Query);

            static IQueryable<AssignRolesToUserRole> Query(IQueryable<Role> query)
            {
                query = query.OrderBy(p => p.Name);

                return query.Select(p => new AssignRolesToUserRole
                {
                    Id = p.Id,
                    Name = p.Name,
                });
            }
        }

        private async Task<IReadOnlyCollection<RoleApplication>> GetAvailableApplicationsAsync()
        {
            return await applicationManager.ListAsync(Query).ToListAsync();

            static IQueryable<RoleApplication> Query(IQueryable<Application> query)
            {
                query = query.OrderBy(p => p.DisplayName);

                return query.Select(p => new RoleApplication
                {
                    Id = p.Id,
                    Name = p.DisplayName,
                });
            }
        }
    }
}
