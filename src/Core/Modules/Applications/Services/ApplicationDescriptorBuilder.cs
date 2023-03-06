// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Services.OpenIddict.Managers;
using Nocturne.Auth.Core.Services.OpenIddict.Services;
using Nocturne.Auth.Core.Shared.Extensions;
using Nocturne.Auth.Core.Shared.Helpers;
using OpenIddict.Abstractions;
using ClientTypes = OpenIddict.Abstractions.OpenIddictConstants.ClientTypes;
using OpenIdPermissions = OpenIddict.Abstractions.OpenIddictConstants.Permissions;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ApplicationDescriptorBuilder
    {
        private readonly ManageApplicationCommand command;
        private readonly IOpenIddictApplicationManager applicationManager;
        private readonly IClientBuilderService clientBuilderService;

        private object application;
        private OpenIddictApplicationDescriptor descriptor;

        public ApplicationDescriptorBuilder(
            ManageApplicationCommand command,
            IOpenIddictApplicationManager applicationManager,
            IClientBuilderService clientBuilderService)
        {
            this.command = command;
            this.applicationManager = applicationManager;
            this.clientBuilderService = clientBuilderService;
        }

        public ApplicationDescriptorBuilder WithApplication(object application)
        {
            this.application = application;

            return this;
        }

        public async Task<OpenIddictApplicationDescriptor> BuildAsync()
        {
            var clientId = application is not null
                ? await applicationManager.GetClientIdAsync(application)
                : clientBuilderService.GenerateClientId();

            var currentClientSecret = application is not null
                ? await applicationManager.GetUnprotectedClientSecret(application)
                : null;

            descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = GetClientSecret(currentClientSecret),
                ConsentType = command.ConsentType,
                DisplayName = command.DisplayName,
                Type = command.Type,
            };

            AddUris();
            AddPermissions();

            return descriptor;
        }

        private void AddUris()
        {
            descriptor.PostLogoutRedirectUris.Clear();
            descriptor.PostLogoutRedirectUris.UnionWith(
                UriHelper.GetUris(command.PostLogoutRedirectUris));

            descriptor.RedirectUris.Clear();
            descriptor.RedirectUris.UnionWith(
                UriHelper.GetUris(command.RedirectUris));
        }

        private void AddPermissions()
        {
            SetPermission(
                command.AllowLogoutEndpoint,
                OpenIdPermissions.Endpoints.Logout);

            SetPermission(
                command.AllowAuthorizationCodeFlow,
                OpenIdPermissions.GrantTypes.AuthorizationCode);

            SetPermission(
                command.AllowClientCredentialsFlow,
                OpenIdPermissions.GrantTypes.ClientCredentials);

            SetPermission(
                command.AllowImplicitFlow,
                OpenIdPermissions.GrantTypes.Implicit);

            SetPermission(
                command.AllowPasswordFlow,
                OpenIdPermissions.GrantTypes.Password);

            SetPermission(
                command.AllowRefreshTokenFlow,
                OpenIdPermissions.GrantTypes.RefreshToken);

            SetPermission(
                command.AllowAuthorizationCodeFlow || command.AllowImplicitFlow,
                OpenIdPermissions.Endpoints.Authorization);

            SetPermission(
                command.AllowAuthorizationCodeFlow ||
                command.AllowClientCredentialsFlow ||
                command.AllowPasswordFlow ||
                command.AllowRefreshTokenFlow,
                OpenIdPermissions.Endpoints.Token);

            SetPermission(
                command.AllowAuthorizationCodeFlow,
                OpenIdPermissions.ResponseTypes.Code);

            SetPermission(
                command.AllowImplicitFlow,
                OpenIdPermissions.ResponseTypes.IdToken);

            SetPermission(
                command.AllowImplicitFlow && IsPublic(command),
                OpenIdPermissions.ResponseTypes.IdTokenToken,
                OpenIdPermissions.ResponseTypes.Token);

            SetPermission(
                command.AllowHybridFlow,
                OpenIdPermissions.ResponseTypes.CodeIdToken);

            SetPermission(
                command.AllowHybridFlow && IsPublic(command),
                OpenIdPermissions.ResponseTypes.CodeIdTokenToken,
                OpenIdPermissions.ResponseTypes.CodeToken);

            RemovePermissionsOfType(OpenIdPermissions.Prefixes.Scope);

            if (command.AllowedScopes != null)
            {
                descriptor.Permissions.UnionWith(
                    command.AllowedScopes
                    .GetDelimitedElements()
                    .Select(s => OpenIdPermissions.Prefixes.Scope + s));
            }
        }

        private void SetPermission(bool allow, params string[] permissions)
        {
            if (allow)
            {
                descriptor.Permissions.AddRange(permissions);
            }
            else
            {
                descriptor.Permissions.RemoveRange(permissions);
            }
        }

        private void RemovePermissionsOfType(string permissionType)
        {
            descriptor.Permissions.RemoveWhere(p => p.StartsWith(permissionType, StringComparison.Ordinal));
        }

        private string GetClientSecret(string currentClientSecret)
        {
            if (IsConfidential(command))
            {
                return currentClientSecret is null
                    ? clientBuilderService.GenerateClientSecret()
                    : currentClientSecret;
            }

            return null;
        }

        private static bool IsPublic(ManageApplicationCommand command)
        {
            return command.Type.IsEqualInvariant(ClientTypes.Public);
        }

        private static bool IsConfidential(ManageApplicationCommand command)
        {
            return command.Type.IsEqualInvariant(ClientTypes.Confidential);
        }
    }
}
