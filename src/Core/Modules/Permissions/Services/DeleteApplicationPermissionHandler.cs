// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Permissions.Repositories;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class DeleteApplicationPermissionHandler
    {
        private readonly IPermissionsRepository permissionsRepository;

        public DeleteApplicationPermissionHandler(
            IPermissionsRepository permissionsRepository)
        {
            this.permissionsRepository = permissionsRepository;
        }

        public async Task<DeleteApplicationPermissionCommand> CreateCommandAsync(long? permissionId)
        {
            var permission = await GetPermissionAsync(permissionId);

            if (permission is null)
            {
                return null;
            }

            return new DeleteApplicationPermissionCommand(permission);
        }

        public async Task<DeleteApplicationPermissionResult> HandleAsync(
            DeleteApplicationPermissionCommand command)
        {
            var permission = await GetPermissionAsync(command.Id);

            if (permission is null)
            {
                return DeleteApplicationPermissionResult.NotFound();
            }

            await permissionsRepository.DeleteAsync(permission);

            return DeleteApplicationPermissionResult.Success();
        }

        private Task<Permission> GetPermissionAsync(long? permissionId)
        {
            if (permissionId.HasValue is false)
            {
                return null;
            }

            return permissionsRepository.GetById(permissionId.Value);
        }
    }
}
