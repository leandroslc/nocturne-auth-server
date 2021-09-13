// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class DeleteApplicationRoleHandler
    {
        private readonly IRolesRepository rolesRepository;

        public DeleteApplicationRoleHandler(
            IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<DeleteApplicationRoleCommand> CreateCommandAsync(long? roleId)
        {
            var role = await GetRoleAsync(roleId);

            if (role is null)
            {
                return null;
            }

            return new DeleteApplicationRoleCommand(role);
        }

        public async Task<DeleteApplicationRoleResult> HandleAsync(
            DeleteApplicationRoleCommand command)
        {
            var role = await GetRoleAsync(command.Id);

            if (role is null)
            {
                return DeleteApplicationRoleResult.NotFound();
            }

            await rolesRepository.DeleteAsync(role);

            return DeleteApplicationRoleResult.Success();
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
