using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Shared.Collections;
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
            IQueryable<ListApplicationsResult> query(IQueryable<Application> applications)
                => GetQuery(applications, command);

            IQueryable<ListApplicationsResult> queryWithPages(IQueryable<Application> applications)
                => GetSubset(query(applications), command);

            var total = await applicationManager.CountAsync(query);
            var applications = applicationManager.ListAsync(queryWithPages);

            return new PagedCollection<ListApplicationsResult>(
                await applications.ToListAsync(), command.Page, command.PageSize, total);
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
