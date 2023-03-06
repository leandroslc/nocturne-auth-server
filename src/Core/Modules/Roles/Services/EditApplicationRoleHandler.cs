// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class EditApplicationRoleHandler : ManageApplicationRoleHandler
    {
        public EditApplicationRoleHandler(
            IStringLocalizer<EditApplicationRoleHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IRolesRepository permissionsRepository)
            : base(localizer, applicationManager, permissionsRepository)
        {
        }

        public async Task<EditApplicationRoleCommand> CreateCommandAsync(long id)
        {
            var role = await GetRoleAsync(id);

            var application = await GetRoleApplicationAsync(role.ApplicationId);

            return new EditApplicationRoleCommand(role, application);
        }

        public async Task<ManageApplicationRoleResult> HandleAsync(EditApplicationRoleCommand command)
        {
            var role = command.Id.HasValue
                ? await GetRoleAsync(command.Id.Value)
                : null;

            if (role is null)
            {
                return ManageApplicationRoleResult.NotFound(Localizer["Role not found"]);
            }

            UpdateRole(role, command);

            if (await RolesRepository.HasDuplicated(role))
            {
                return ManageApplicationRoleResult.Duplicated(
                    Localizer["The role {0} already exists", role.Name]);
            }

            try
            {
                await RolesRepository.UpdateAsync(role);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ManageApplicationRoleResult.Fail(
                    Localizer["The permission has been modified. Check the data and try again"]);
            }

            return ManageApplicationRoleResult.Success(role.Id, role.ApplicationId);
        }

        public async Task<bool> RoleExsits(long? id)
        {
            return id.HasValue && await RolesRepository.Exists(id.Value);
        }

        private async Task<Role> GetRoleAsync(long id)
        {
            return await RolesRepository.GetById(id);
        }

        private static void UpdateRole(
            Role role,
            EditApplicationRoleCommand command)
        {
            role.SetName(command.Name);
            role.SetDescription(command.Description);
        }
    }
}
