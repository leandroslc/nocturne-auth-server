using System.Collections.Generic;
using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class EditApplicationHandler : ManageApplicationHandler<EditApplicationCommand>
    {
        public EditApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager)
            : base(applicationManager, scopeManager)
        {
        }

        public override async Task HandleAsync(EditApplicationCommand command)
        {
            var application = await GetApplicationAsync(command.Id);

            var descriptor = await GenerateDescriptorAsync(command, application);

            await ApplicationManager.UpdateAsync(application, descriptor);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await GetApplicationAsync(id) is not null;
        }

        public async Task<EditApplicationCommand> CreateCommandAsync(string id)
        {
            var application = await GetApplicationAsync(id);

            var command = new EditApplicationCommand
            {
                Id = await ApplicationManager.GetIdAsync(application),
                ConsentType = await ApplicationManager.GetConsentTypeAsync(application),
                DisplayName = await ApplicationManager.GetDisplayNameAsync(application),
                Type = await ApplicationManager.GetClientTypeAsync(application),
                PostLogoutRedirectUris = string.Join(' ', await ApplicationManager.GetPostLogoutRedirectUrisAsync(application)),
                RedirectUris = string.Join(' ', await ApplicationManager.GetRedirectUrisAsync(application)),
                AllowAuthorizationCodeFlow = await HasPermissionAsync(application, Permissions.GrantTypes.AuthorizationCode),
                AllowClientCredentialsFlow = await HasPermissionAsync(application, Permissions.GrantTypes.ClientCredentials),
                AllowImplicitFlow = await HasPermissionAsync(application, Permissions.GrantTypes.Implicit),
                AllowPasswordFlow = await HasPermissionAsync(application, Permissions.GrantTypes.Password),
                AllowRefreshTokenFlow = await HasPermissionAsync(application, Permissions.GrantTypes.RefreshToken),
                AllowLogoutEndpoint = await HasPermissionAsync(application, Permissions.Endpoints.Logout),
            };

            await AddAvailableScopesAsync(command);

            command.AllowedScopes = await GetAllowedScopes(application, command.AvailableScopes);

            return command;
        }

        private async ValueTask<string> GetAllowedScopes(
            object application,
            IReadOnlyCollection<string> availableScopes)
        {
            var selectedScopes = new HashSet<string>();

            foreach (var scopeName in availableScopes)
            {
                if (await HasPermissionAsync(application, Permissions.Prefixes.Scope + scopeName))
                {
                    selectedScopes.Add(scopeName);
                }
            }

            return string.Join(' ', selectedScopes);
        }

        private async Task<object> GetApplicationAsync(string id)
        {
            return await ApplicationManager.FindByIdAsync(id);
        }

        private ValueTask<bool> HasPermissionAsync(object application, string permission)
        {
            return ApplicationManager.HasPermissionAsync(application, permission);
        }
    }
}
