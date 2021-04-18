using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Results;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class ViewApplicationHandler
    {
        private readonly IOpenIddictApplicationManager applicationManager;
        private readonly IOpenIddictScopeManager scopeManager;

        public ViewApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager)
        {
            this.applicationManager = applicationManager;
            this.scopeManager = scopeManager;
        }

        public async ValueTask<ViewApplicationResult> HandleAsync(ViewApplicationCommand command)
        {
            var application = await GetApplicationAsync(command.Id);

            var result = new ViewApplicationResult
            {
                Id = command.Id,
                ClientId = await applicationManager.GetClientIdAsync(application),
                ClientSecret = await applicationManager.GetUnprotectedClientSecret(application),
                DisplayName = await applicationManager.GetDisplayNameAsync(application),
                ConsentType = await applicationManager.GetConsentTypeAsync(application),
                Type = await applicationManager.GetClientTypeAsync(application),
                PostLogoutRedirectUris = await applicationManager.GetPostLogoutRedirectUrisAsync(application),
                RedirectUris = await applicationManager.GetRedirectUrisAsync(application),
                AllowAuthorizationCodeFlow = await HasPermissionAsync(application, Permissions.GrantTypes.AuthorizationCode),
                AllowClientCredentialsFlow = await HasPermissionAsync(application, Permissions.GrantTypes.ClientCredentials),
                AllowImplicitFlow = await HasPermissionAsync(application, Permissions.GrantTypes.Implicit),
                AllowPasswordFlow = await HasPermissionAsync(application, Permissions.GrantTypes.Password),
                AllowRefreshTokenFlow = await HasPermissionAsync(application, Permissions.GrantTypes.RefreshToken),
                AllowLogoutEndpoint = await HasPermissionAsync(application, Permissions.Endpoints.Logout),
            };

            await AddAllowedScopesAsync(application, result);

            return result;
        }

        public async ValueTask<bool> ExistsAsync(ViewApplicationCommand command)
        {
            return await GetApplicationAsync(command.Id) is not null;
        }

        private ValueTask<object> GetApplicationAsync(string id)
        {
            return applicationManager.FindByIdAsync(id);
        }

        private async Task AddAllowedScopesAsync(
            object application,
            ViewApplicationResult result)
        {
            await foreach (var scope in scopeManager.ListAsync())
            {
                var scopeName = await scopeManager.GetNameAsync(scope);

                if (await HasPermissionAsync(application, Permissions.Prefixes.Scope + scopeName))
                {
                    result.AllowedScopes.Add(scopeName);
                }
            }
        }

        private ValueTask<bool> HasPermissionAsync(object application, string permission)
        {
            return applicationManager.HasPermissionAsync(application, permission);
        }
    }
}
