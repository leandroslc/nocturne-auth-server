// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class DeleteRoleHandler
    {
        private readonly IRolesRepository rolesRepository;

        public DeleteRoleHandler(
            IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<DeleteRoleCommand> CreateCommandAsync(long? roleId)
        {
            var role = await GetRoleAsync(roleId);

            if (role is null)
            {
                return null;
            }

            return new DeleteRoleCommand(role);
        }

        public async Task<DeleteRoleResult> HandleAsync(DeleteRoleCommand command)
        {
            var role = await GetRoleAsync(command.Id);

            if (role is null)
            {
                return DeleteRoleResult.NotFound();
            }

            await rolesRepository.DeleteAsync(role);

            return DeleteRoleResult.Success();
        }

        private Task<Role> GetRoleAsync(long? roleId)
        {
            if (roleId.HasValue is false)
            {
                return null;
            }

            return rolesRepository.GetById(roleId.Value);
        }
    }
}
