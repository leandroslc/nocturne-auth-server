// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class EditRoleHandler : ManageRoleHandler
    {
        public EditRoleHandler(
            IStringLocalizer<EditRoleHandler> localizer,
            IRolesRepository permissionsRepository)
            : base(localizer, permissionsRepository)
        {
        }

        public async Task<EditRoleCommand> CreateCommandAsync(long id)
        {
            var role = await GetRoleAsync(id);

            return new EditRoleCommand(role);
        }

        public async Task<ManageRoleResult> HandleAsync(EditRoleCommand command)
        {
            var role = command.Id.HasValue
                ? await GetRoleAsync(command.Id.Value)
                : null;

            if (role is null)
            {
                return ManageRoleResult.NotFound(Localizer["Role not found"]);
            }

            UpdateRole(role, command);

            if (await RolesRepository.HasDuplicated(role))
            {
                return ManageRoleResult.Duplicated(
                    Localizer["The role {0} already exists", role.Name]);
            }

            try
            {
                await RolesRepository.UpdateAsync(role);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ManageRoleResult.Fail(
                    Localizer["The role has been modified. Check the data and try again"]);
            }

            return ManageRoleResult.Success(role.Id);
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
            EditRoleCommand command)
        {
            role.SetName(command.Name);
            role.SetDescription(command.Description);
        }
    }
}
