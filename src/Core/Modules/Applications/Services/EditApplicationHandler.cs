// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Services.OpenIddict.Services;
using OpenIddict.Abstractions;
using ConcurrencyException = OpenIddict.Abstractions.OpenIddictExceptions.ConcurrencyException;
using OpenIdPermissions = OpenIddict.Abstractions.OpenIddictConstants.Permissions;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class EditApplicationHandler : ManageApplicationHandler<EditApplicationCommand>
    {
        public EditApplicationHandler(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictScopeManager scopeManager,
            IClientBuilderService clientBuilderService,
            IStringLocalizer<EditApplicationHandler> localizer)
            : base(applicationManager, scopeManager, clientBuilderService, localizer)
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
                AllowAuthorizationCodeFlow = await HasPermissionAsync(application, OpenIdPermissions.GrantTypes.AuthorizationCode),
                AllowClientCredentialsFlow = await HasPermissionAsync(application, OpenIdPermissions.GrantTypes.ClientCredentials),
                AllowImplicitFlow = await HasPermissionAsync(application, OpenIdPermissions.GrantTypes.Implicit),
                AllowPasswordFlow = await HasPermissionAsync(application, OpenIdPermissions.GrantTypes.Password),
                AllowRefreshTokenFlow = await HasPermissionAsync(application, OpenIdPermissions.GrantTypes.RefreshToken),
                AllowLogoutEndpoint = await HasPermissionAsync(application, OpenIdPermissions.Endpoints.Logout),
            };

            await AddAvailableScopesAsync(command);

            command.AllowedScopes = await GetAllowedScopes(application, command.AvailableScopes);

            return command;
        }

        public async Task<EditApplicationResult> HandleAsync(EditApplicationCommand command)
        {
            var application = await GetApplicationAsync(command.Id);

            if (application is null)
            {
                return EditApplicationResult.NotFound();
            }

            if (await HasDuplicatedApplication(command, application))
            {
                return EditApplicationResult.Fail(
                    Localizer["Application {0} already exists", command.DisplayName]!);
            }

            var descriptor = await CreateApplicationDescriptorBuilder(command)
                .WithApplication(application)
                .BuildAsync();

            try
            {
                await ApplicationManager.UpdateAsync(application, descriptor);
            }
            catch (ConcurrencyException)
            {
                EditApplicationResult.Fail(
                    Localizer["The application has been modified externally. Check the data and try again"]!);
            }

            return EditApplicationResult.Updated(command.Id);
        }

        public async Task<bool> ExistsAsync(string applicationId)
        {
            return await GetApplicationAsync(applicationId) is not null;
        }

        private async ValueTask<string> GetAllowedScopes(
            object application,
            IEnumerable<string> availableScopes)
        {
            var selectedScopes = new HashSet<string>();

            foreach (var scopeName in availableScopes)
            {
                if (await HasPermissionAsync(application, OpenIdPermissions.Prefixes.Scope + scopeName))
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
