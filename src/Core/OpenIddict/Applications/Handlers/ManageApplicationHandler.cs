using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Helpers;
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

        public virtual async IAsyncEnumerable<ValidationResult> ValidateAsync(TCommand command)
        {
            var validationResults = UriHelper.Validate(
                command.RedirectUris,
                nameof(command.RedirectUris),
                InvalidUri);

            validationResults = validationResults.Union(
                UriHelper.Validate(
                    command.PostLogoutRedirectUris,
                    nameof(command.PostLogoutRedirectUris),
                    InvalidUri));

            foreach (var result in validationResults)
            {
                yield return result;
            }

            if (validationResults.Any())
            {
                yield break;
            }

            await Task.CompletedTask;
        }

        protected async Task AddAvailableScopesAsync(TCommand command)
        {
            var scopes = ScopeManager.ListAsync();

            await foreach (var scope in scopes)
            {
                command.AvailableScopes.Add(await ScopeManager.GetNameAsync(scope));
            }
        }

        private static string InvalidUri(string url)
        {
            return $"{url} is invalid";
        }
    }
}
