using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Permissions.Repositories;
using Nocturne.Auth.Core.Services.OpenIddict;
using Nocturne.Auth.Core.Services.OpenIddict.Managers;

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public abstract class ManagePermissionHandler
    {
        public ManagePermissionHandler(
            IStringLocalizer<ManagePermissionHandler> localizer,
            CustomOpenIddictApplicationManager<Application> applicationManager,
            IPermissionsRepository permissionsRepository)
        {
            Localizer = localizer;
            ApplicationManager = applicationManager;
            PermissionsRepository = permissionsRepository;
        }

        protected IStringLocalizer Localizer { get; }

        protected CustomOpenIddictApplicationManager<Application> ApplicationManager { get; }

        protected IPermissionsRepository PermissionsRepository { get; }

        protected async Task<Application> GetApplication(string applicationId)
        {
            return await ApplicationManager.FindByIdAsync(applicationId);
        }
    }
}