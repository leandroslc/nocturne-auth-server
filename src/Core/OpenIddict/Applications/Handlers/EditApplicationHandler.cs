using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.Results;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Abstractions.OpenIddictExceptions;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class EditApplicationHandler : ManageApplicationHandler<EditApplicationCommand>
    {
        public EditApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IStringLocalizer<EditApplicationHandler> localizer)
            : base(applicationManager, scopeManager, localizer)
        {
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

        public async Task<Result> HandleAsync(EditApplicationCommand command)
        {
            var application = await GetApplicationAsync(command.Id);

            if (application is null)
            {
                return Result.NotFound();
            }

            if (await HasDuplicatedApplication(command, application))
            {
                return Result.Fail(Localizer["Application {0} already exists", command.DisplayName]!);
            }

            var descriptor = await new ApplicationDescriptorBuilder(
                command, ApplicationManager)
                .WithApplication(application)
                .BuildAsync();

            try
            {
                await ApplicationManager.UpdateAsync(application, descriptor);
            }
            catch (ConcurrencyException)
            {
                Result.Fail(
                    Localizer["The application has been modified externally. Check the data and try again"]!);
            }

            return Result.Success;
        }

        public async Task<bool> ExistsAsync(string applicationId)
        {
            return await GetApplicationAsync(applicationId) is not null;
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
