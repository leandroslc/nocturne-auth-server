// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class CreatePermissionHandler : ManagePermissionHandler
    {
        public CreatePermissionHandler(
            IStringLocalizer<CreatePermissionHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IPermissionsRepository permissionsRepository)
            : base(localizer, applicationManager, permissionsRepository)
        {
        }

        public async Task<ManagePermissionResult> HandleAsync(CreatePermissionCommand command)
        {
            var application = await GetApplication(command.ApplicationId);

            if (application is null)
            {
                return ManagePermissionResult.NotFound(Localizer["Application not found"]);
            }

            var permission = CreatePermission(command);

            if (await PermissionsRepository.HasDuplicated(permission))
            {
                return ManagePermissionResult.Duplicated(
                    Localizer["The permission {0} already exists", permission.Name]);
            }

            await PermissionsRepository.InsertAsync(permission);

            return ManagePermissionResult.Success(permission.Id);
        }

        public async Task<bool> ApplicationExists(string applicationId)
        {
            if (applicationId is null)
            {
                return false;
            }

            return await GetApplication(applicationId) is not null;
        }

        private static Permission CreatePermission(CreatePermissionCommand command)
        {
            return new Permission(
                name: command.Name,
                applicationId: command.ApplicationId,
                description: command.Description);
        }
    }
}
