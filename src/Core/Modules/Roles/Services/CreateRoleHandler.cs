// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class CreateRoleHandler : ManageRoleHandler
    {
        public CreateRoleHandler(
            IStringLocalizer<CreateRoleHandler> localizer,
            IRolesRepository rolesRepository)
            : base(localizer, rolesRepository)
        {
        }

        public async Task<ManageRoleResult> HandleAsync(CreateRoleCommand command)
        {
            var role = CreateRole(command);

            if (await RolesRepository.HasDuplicated(role))
            {
                return ManageRoleResult.Duplicated(
                    Localizer["The role {0} already exists", role.Name]);
            }

            await RolesRepository.InsertAsync(role);

            return ManageRoleResult.Success(role.Id);
        }

        private static Role CreateRole(CreateRoleCommand command)
        {
            return new Role(
                name: command.Name,
                description: command.Description);
        }
    }
}
