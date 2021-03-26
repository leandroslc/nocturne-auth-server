using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;
using OpenIddict.Core;
using Application = OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication<long>;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class ListApplicationsHandler
    {
        private readonly OpenIddictApplicationManager<Application> applicationManager;

        public ListApplicationsHandler(
            OpenIddictApplicationManager<Application> applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public async ValueTask<IReadOnlyCollection<ListApplicationsResult>> HandleAsync(
            ListApplicationsCommand command)
        {
            IQueryable<ListApplicationsResult> query(IQueryable<Application> applications)
                => GetQuery(applications, command);

            IQueryable<ListApplicationsResult> queryWithPages(IQueryable<Application> applications)
                => GetSubset(query(applications), command);

            var total = await applicationManager.CountAsync(query);
            var applications = applicationManager.ListAsync(queryWithPages);

            return await applications.ToListAsync();
        }

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
