// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class EditPermissionHandler : ManagePermissionHandler
    {
        public EditPermissionHandler(
            IStringLocalizer<EditPermissionHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IPermissionsRepository permissionsRepository)
            : base(localizer, applicationManager, permissionsRepository)
        {
        }

        public async Task<EditPermissionCommand> CreateCommandAsync(long id)
        {
            var permission = await GetPermission(id);

            return new EditPermissionCommand(permission);
        }

        public async Task<ManagePermissionResult> HandleAsync(EditPermissionCommand command)
        {
            var permission = command.Id.HasValue
                ? await GetPermission(command.Id.Value)
                : null;

            if (permission is null)
            {
                return ManagePermissionResult.NotFound(Localizer["Permission not found"]);
            }

            UpdatePermission(permission, command);

            if (await PermissionsRepository.HasDuplicated(permission))
            {
                return ManagePermissionResult.Duplicated(
                    Localizer["The permission {0} already exists", permission.Name]);
            }

            try
            {
                await PermissionsRepository.UpdateAsync(permission);
            }
            catch (DbUpdateConcurrencyException)
            {
                return ManagePermissionResult.Fail(
                    Localizer["The permission has been modified. Check the data and try again"]);
            }

            return ManagePermissionResult.Success(permission.Id);
        }

        public Task<bool> PermissionExsits(long id)
        {
            return PermissionsRepository.Exists(id);
        }

        private async Task<Permission> GetPermission(long id)
        {
            return await PermissionsRepository.GetById(id);
        }

        private static void UpdatePermission(
            Permission permission,
            EditPermissionCommand command)
        {
            permission.SetName(command.Name);
            permission.SetDescription(command.Description);
        }
    }
}
