// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewApplicationRoleHandler
    {
        private readonly CustomOpenIddictApplicationManager<Application> applicationManager;
        private readonly IRolesRepository rolesRepository;

        public ViewApplicationRoleHandler(
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IRolesRepository rolesRepository)
        {
            this.applicationManager = applicationManager;
            this.rolesRepository = rolesRepository;
        }

        public async Task<ViewApplicationRoleResult> HandleAsync(
            ViewApplicationRoleCommand command)
        {
            var role = await GetRoleAsync(command.Id);

            if (role is null)
            {
                return ViewApplicationRoleResult.NotFound();
            }

            var application = await GetApplicationAsync(role);

            return ViewApplicationRoleResult.Success(role, application);
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            if (id.HasValue is false)
            {
                return null;
            }

            return await rolesRepository.GetById(id.Value);
        }

        private async Task<RoleApplication> GetApplicationAsync(Role role)
        {
            var application = await applicationManager.FindByIdAsync(role.ApplicationId);

            return new RoleApplication(application.Id, application.DisplayName);
        }
    }
}
