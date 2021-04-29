using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class CreateApplicationRoleHandler : ManageApplicationRoleHandler
    {
        public CreateApplicationRoleHandler(
            IStringLocalizer<CreateApplicationRoleHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IRolesRepository rolesRepository)
            : base(localizer, applicationManager, rolesRepository)
        {
        }

        public async Task<CreateApplicationRoleCommand> CreateCommandAsync(string applicationId)
        {
            var application = await GetRoleApplicationAsync(applicationId);

            return new CreateApplicationRoleCommand(application);
        }

        public async Task<ManageApplicationRoleResult> HandleAsync(CreateApplicationRoleCommand command)
        {
            var application = await GetApplicationAsync(command.ApplicationId);

            if (application is null)
            {
                return ManageApplicationRoleResult.NotFound(Localizer["Application not found"]);
            }

            var role = CreateRole(command);

            if (await RolesRepository.HasDuplicated(role))
            {
                return ManageApplicationRoleResult.Duplicated(
                    Localizer["The role {0} already exists", role.Name]);
            }

            await RolesRepository.InsertAsync(role);

            return ManageApplicationRoleResult.Success(role.Id, role.ApplicationId);
        }

        public async Task<bool> ApplicationExists(string applicationId)
        {
            return applicationId != null
                && await GetApplicationAsync(applicationId) is not null;
        }

        private static Role CreateRole(CreateApplicationRoleCommand command)
        {
            return new Role(
                name: command.Name,
                applicationId: command.ApplicationId,
                description: command.Description);
        }
    }
}
