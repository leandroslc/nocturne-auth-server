using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public abstract class ManageApplicationHandler<TCommand>
        where TCommand : ManageApplicationCommand
    {
        public ManageApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager)
        {
            ApplicationManager = applicationManager;
            ScopeManager = scopeManager;
        }

        protected IOpenIddictApplicationManager ApplicationManager { get; }

        protected IOpenIddictScopeManager ScopeManager { get; }

        public abstract Task HandleAsync(TCommand command);

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
