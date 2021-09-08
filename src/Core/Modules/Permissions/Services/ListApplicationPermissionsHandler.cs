// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ListApplicationPermissionsHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IOpenIddictApplicationManager applicationManager;
        private readonly IPermissionsRepository permissionsRepository;

        public ListApplicationPermissionsHandler(
            IStringLocalizer<ListApplicationPermissionsHandler> localizer,
            IOpenIddictApplicationManager applicationManager,
            IPermissionsRepository permissionsRepository)
        {
            this.localizer = localizer;
            this.applicationManager = applicationManager;
            this.permissionsRepository = permissionsRepository;
        }

        public async Task<ListApplicationPermissionsResult> HandleAsync(
            ListApplicationPermissionsCommand command)
        {
            var application = await applicationManager.FindByIdAsync(command.ApplicationId);

            if (application is null)
            {
                return ListApplicationPermissionsResult.NotFound(
                    localizer["Application not found"]);
            }

            var permissions = await permissionsRepository.QueryByApplication(
                command.ApplicationId, query => GetPermissions(query, command));

            return ListApplicationPermissionsResult.Success(permissions);
        }

        private static IQueryable<ListApplicationPermissionsItem> GetPermissions(
            IQueryable<Permission> query,
            ListApplicationPermissionsCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name) is false)
            {
                query = query.Where(p => p.Name.Contains(command.Name));
            }

            query = query.OrderBy(p => p.Name);

            return query.Select(p => new ListApplicationPermissionsItem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });
        }
    }
}
