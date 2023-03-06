// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public abstract class ManageApplicationRoleHandler
    {
        protected ManageApplicationRoleHandler(
            IStringLocalizer<ManageApplicationRoleHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IRolesRepository rolesRepository)
        {
            Localizer = localizer;
            ApplicationManager = applicationManager;
            RolesRepository = rolesRepository;
        }

        protected IStringLocalizer Localizer { get; }

        protected CustomOpenIddictApplicationManager<Application> ApplicationManager { get; }

        protected IRolesRepository RolesRepository { get; }

        protected async Task<RoleApplication> GetRoleApplicationAsync(string applicationId)
        {
            var application = await GetApplicationAsync(applicationId);

            return application is null
                ? null
                : new RoleApplication(application.Id, application.DisplayName);
        }

        protected async Task<Application> GetApplicationAsync(string applicationId)
        {
            return await ApplicationManager.FindByIdAsync(applicationId);
        }
    }
}
