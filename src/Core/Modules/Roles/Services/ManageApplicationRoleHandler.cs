using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public abstract class ManageApplicationRoleHandler
    {
        public ManageApplicationRoleHandler(
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

        protected async Task<Application> GetApplication(string applicationId)
        {
            return await ApplicationManager.FindByIdAsync(applicationId);
        }
    }
}
