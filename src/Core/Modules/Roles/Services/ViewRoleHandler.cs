// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewRoleHandler
    {
        private readonly IRolesRepository rolesRepository;

        public ViewRoleHandler(IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<ViewRoleResult> HandleAsync(ViewRoleCommand command)
        {
            var role = await GetRoleAsync(command.Id);

            if (role is null)
            {
                return ViewRoleResult.NotFound();
            }

            return ViewRoleResult.Success(role);
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            if (id.HasValue is false)
            {
                return null;
            }

            return await rolesRepository.GetById(id.Value);
        }
    }
}
