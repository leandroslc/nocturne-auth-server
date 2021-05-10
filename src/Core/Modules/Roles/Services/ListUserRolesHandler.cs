using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ListUserRolesHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRolesRepository userRolesRepository;

        public ListUserRolesHandler(
            IStringLocalizer<ListUserRolesHandler> localizer,
            UserManager<ApplicationUser> userManager,
            IUserRolesRepository userRolesRepository)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.userRolesRepository = userRolesRepository;
        }

        public async Task<ListUserRolesResult> HandleAsync(
            ListUserRolesCommand command)
        {
            var user = await GetUserAsync(command.UserId);

            if (user is null)
            {
                return ListUserRolesResult.NotFound(localizer["User not found"]);
            }

            var query = GetRolesQuery(
                userRolesRepository.QueryByUser(user.Id),
                command);

            var total = await query.LongCountAsync();
            var roles = await GetSubset(query, command).ToListAsync();

            var collection = new PagedCollection<ListUserRolesItem>(
                roles, command.Page, command.PageSize, total);

            return ListUserRolesResult.Success(collection);
        }

        private async Task<ApplicationUser> GetUserAsync(long? id)
        {
            return id.HasValue
                ? await userManager.FindByIdAsync(id.Value.ToString())
                : null;
        }

        private static IQueryable<ListUserRolesItem> GetRolesQuery(
            IQueryable<Role> query,
            ListUserRolesCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name) is false)
            {
                query = query.Where(p => p.Name.Contains(command.Name));
            }

            query = query
                .OrderBy(p => p.Application.DisplayName)
                .ThenBy(p => p.Name);

            return query.Select(p => new ListUserRolesItem
            {
                Id = p.Id,
                Name = p.Name,
                ApplicationId = p.Application.Id,
                ApplicationName = p.Application.DisplayName,
            });
        }

        private static IQueryable<ListUserRolesItem> GetSubset(
            IQueryable<ListUserRolesItem> query,
            ListUserRolesCommand command)
        {
            return query
                .Skip((command.Page - 1) * command.PageSize)
                .Take(command.PageSize);
        }
    }
}
