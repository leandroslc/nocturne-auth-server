using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserHandler
    {
        private readonly IStringLocalizer localizer;
        private readonly IRolesRepository rolesRepository;
        private readonly IUserRolesRepository userRolesRepository;
        private readonly CustomOpenIddictApplicationManager<Application> applicationManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AssignRolesToUserHandler(
            IStringLocalizer<AssignRolesToUserHandler> localizer,
            IRolesRepository rolesRepository,
            IUserRolesRepository userRolesRepository,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            UserManager<ApplicationUser> userManager)
        {
            this.localizer = localizer;
            this.rolesRepository = rolesRepository;
            this.userRolesRepository = userRolesRepository;
            this.applicationManager = applicationManager;
            this.userManager = userManager;
        }

        public async Task<AssignRolesToUserCommand> CreateCommandAsync(
            long? userId,
            string applicationId = null)
        {
            var user = await GetUserAsync(userId);

            var availableRoles = applicationId is not null
                ? null
                : await GetAvailableApplicationRolesAsync(applicationId);

            var command = new AssignRolesToUserCommand(
                applicationId,
                await GetAvailableApplicationsAsync())
            {
                UserId = user.Id,
                Roles = availableRoles,
            };

            await SetSelectedRolesAsync(command, user);

            return command;
        }

        public async Task<AssignRolesToUserResult> HandleAsync(AssignRolesToUserCommand command)
        {
            var user = await GetUserAsync(command.UserId);

            if (user is null)
            {
                return AssignRolesToUserResult.NotFound(localizer["User not found"]);
            }

            var selectedRoles = command.Roles?.Where(p => p.Selected)
                ?? Enumerable.Empty<AssignRolesToUserRole>();

            var rolesToAssign = await GetUnassignedRolesAsync(user, selectedRoles);

            await userRolesRepository.AssignRolesAsync(user, rolesToAssign);

            return AssignRolesToUserResult.Success();
        }

        public async Task<bool> UserExistsAsync(long? id)
        {
            return id.HasValue && await userManager.FindByIdAsync(id.Value.ToString()) is not null;
        }

        private async Task<ApplicationUser> GetUserAsync(long? id)
        {
            return id.HasValue
                ? await userManager.FindByIdAsync(id.Value.ToString())
                : null;
        }

        private async Task SetSelectedRolesAsync(
            AssignRolesToUserCommand command,
            ApplicationUser user)
        {
            var unassignedRoles = await GetUnassignedRolesAsync(user, command.Roles);
            var unassignedRoleIds = unassignedRoles.Select(p => p.Id).ToHashSet();

            foreach (var role in command.Roles)
            {
                if (unassignedRoleIds.Contains(role.Id) is false)
                {
                    role.Selected = true;
                }
            }
        }

        private async Task<IReadOnlyCollection<Role>> GetUnassignedRolesAsync(
            ApplicationUser user,
            IEnumerable<AssignRolesToUserRole> roles)
        {
            var roleIds = roles.Select(p => p.Id);

            return await userRolesRepository.GetUnassignedRolesAsync(user, roleIds);
        }

        private Task<IReadOnlyCollection<AssignRolesToUserRole>> GetAvailableApplicationRolesAsync(
            string applicationId)
        {
            return rolesRepository.QueryByApplication(applicationId, Query);

            static IQueryable<AssignRolesToUserRole> Query(IQueryable<Role> query)
            {
                query = query.OrderBy(p => p.Name);

                return query.Select(p => new AssignRolesToUserRole
                {
                    Id = p.Id,
                    Name = p.Name,
                });
            }
        }

        private async Task<IReadOnlyCollection<RoleApplication>> GetAvailableApplicationsAsync()
        {
            return await applicationManager.ListAsync(Query).ToListAsync();

            static IQueryable<RoleApplication> Query(IQueryable<Application> query)
            {
                query = query.OrderBy(p => p.DisplayName);

                return query.Select(p => new RoleApplication
                {
                    Id = p.Id,
                    Name = p.DisplayName,
                });
            }
        }
    }
}
