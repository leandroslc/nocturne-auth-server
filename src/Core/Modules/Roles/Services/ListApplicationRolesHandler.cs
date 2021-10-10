// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListApplicationRolesHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IOpenIddictApplicationManager applicationManager;
        private readonly IRolesRepository rolesRepository;

        public ListApplicationRolesHandler(
            IStringLocalizer<ListApplicationRolesHandler> localizer,
            IOpenIddictApplicationManager applicationManager,
            IRolesRepository rolesRepository)
        {
            this.localizer = localizer;
            this.applicationManager = applicationManager;
            this.rolesRepository = rolesRepository;
        }

        public async Task<ListApplicationRolesResult> HandleAsync(
            ListApplicationRolesCommand command)
        {
            var application = await applicationManager.FindByIdAsync(command.ApplicationId);

            if (application is null)
            {
                return ListApplicationRolesResult.NotFound(
                    localizer["Application not found"]);
            }

            var roles = await rolesRepository.QueryByApplication(
                command.ApplicationId, query => GetRoles(query, command));

            return ListApplicationRolesResult.Success(roles);
        }

        [SuppressMessage("Globalization", "CA1307", Justification = "Will fail within LINQ provider")]
        private static IQueryable<ListApplicationRolesItem> GetRoles(
            IQueryable<Role> query,
            ListApplicationRolesCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name) is false)
            {
                query = query.Where(p => p.Name.Contains(command.Name));
            }

            query = query.OrderBy(p => p.Name);

            return query.Select(p => new ListApplicationRolesItem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });
        }
    }
}
