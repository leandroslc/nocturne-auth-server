// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Shared.Collections;
using Nocturne.Auth.Core.Shared.Extensions;
using OpenIddict.Core;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ListApplicationsHandler
    {
        private readonly OpenIddictApplicationManager<Application> applicationManager;

        public ListApplicationsHandler(
            OpenIddictApplicationManager<Application> applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public async ValueTask<IPagedCollection<ListApplicationsResult>> HandleAsync(
            ListApplicationsCommand command)
        {
            IQueryable<ListApplicationsResult> Query(IQueryable<Application> applications)
            {
                return GetQuery(applications, command);
            }

            IQueryable<ListApplicationsResult> QueryWithPages(IQueryable<Application> applications)
            {
                return GetSubset(Query(applications), command);
            }

            var total = await applicationManager.CountAsync(Query);
            var applications = applicationManager.ListAsync(QueryWithPages);

            return new PagedCollection<ListApplicationsResult>(
                await applications.ToListAsync(), command.Page, command.PageSize, total);
        }

        [SuppressMessage("Globalization", "CA1310", Justification = "Will fail within LINQ provider")]
        private static IQueryable<ListApplicationsResult> GetQuery(
            IQueryable<Application> applications,
            ListApplicationsCommand command)
        {
            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                applications = applications.Where(a => a.DisplayName.StartsWith(command.Name));
            }

            applications = applications.OrderBy(a => a.DisplayName);

            return applications.Select(a => new ListApplicationsResult
            {
                Id = a.Id,
                Name = a.DisplayName,
                ClientId = a.ClientId,
            });
        }

        private static IQueryable<ListApplicationsResult> GetSubset(
            IQueryable<ListApplicationsResult> query,
            ListApplicationsCommand command)
        {
            return query
                .Skip((command.Page - 1) * command.PageSize)
                .Take(command.PageSize);
        }
    }
}
