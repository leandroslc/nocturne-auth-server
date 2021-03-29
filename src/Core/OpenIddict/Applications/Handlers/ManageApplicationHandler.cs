using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public abstract class ManageApplicationHandler<TCommand>
        where TCommand : ManageApplicationCommand
    {
        public ManageApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IStringLocalizer<ManageApplicationHandler<TCommand>> localizer)
        {
            ApplicationManager = applicationManager;
            ScopeManager = scopeManager;
            Localizer = localizer;
        }

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        protected IStringLocalizer Localizer { get; }

        protected async Task<string> FindDuplicatedApplicationId(TCommand command)
        {
            var application = await ApplicationManager.FindByNameAsync(command.DisplayName);

            return application is null
                ? null
                : await ApplicationManager.GetIdAsync(application);
        }

        protected async Task AddAvailableScopesAsync(TCommand command)
        {
            var scopes = ScopeManager.ListAsync();

            await foreach (var scope in scopes)
            {
                command.AvailableScopes.Add(await ScopeManager.GetNameAsync(scope));
            }
        }
    }
}
