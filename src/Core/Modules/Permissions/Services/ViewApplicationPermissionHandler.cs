// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ViewApplicationPermissionHandler
    {
        private readonly IPermissionsRepository permissionsRepository;

        public ViewApplicationPermissionHandler(
            IPermissionsRepository permissionsRepository)
        {
            this.permissionsRepository = permissionsRepository;
        }

        public async Task<ViewApplicationPermissionResult> HandleAsync(
            ViewApplicationPermissionCommand command)
        {
            var permission = await GetPermissionAsync(command.Id);

            if (permission is null)
            {
                return ViewApplicationPermissionResult.NotFound();
            }

            return ViewApplicationPermissionResult.Success(permission);
        }

        private async Task<Permission> GetPermissionAsync(long? id)
        {
            if (id.HasValue is false)
            {
                return null;
            }

            return await permissionsRepository.GetById(id.Value);
        }
    }
}
