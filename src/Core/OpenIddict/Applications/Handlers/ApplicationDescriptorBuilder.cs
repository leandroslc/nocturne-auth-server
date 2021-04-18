using System;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.Shared.Extensions;
using Nocturne.Auth.Core.Shared.Helpers;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Core.OpenIddict.Applications.Handlers
{
    public class ApplicationDescriptorBuilder
    {
        private readonly ManageApplicationCommand command;
        private readonly IOpenIddictApplicationManager applicationManager;

        private object application;
        private OpenIddictApplicationDescriptor descriptor;

        public ApplicationDescriptorBuilder(
            ManageApplicationCommand command,
            IOpenIddictApplicationManager applicationManager)
        {
            this.command = command;
            this.applicationManager = applicationManager;
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
                : Guid.NewGuid().ToString();

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
                Permissions.Endpoints.Logout);

            SetPermission(
                command.AllowAuthorizationCodeFlow,
                Permissions.GrantTypes.AuthorizationCode);

            SetPermission(
                command.AllowClientCredentialsFlow,
                Permissions.GrantTypes.ClientCredentials);

            SetPermission(
                command.AllowImplicitFlow,
                Permissions.GrantTypes.Implicit);

            SetPermission(
                command.AllowPasswordFlow,
                Permissions.GrantTypes.Password);

            SetPermission(
                command.AllowRefreshTokenFlow,
                Permissions.GrantTypes.RefreshToken);

            SetPermission(
                command.AllowAuthorizationCodeFlow || command.AllowImplicitFlow,
                Permissions.Endpoints.Authorization);

            SetPermission(
                command.AllowAuthorizationCodeFlow ||
                command.AllowClientCredentialsFlow ||
                command.AllowPasswordFlow ||
                command.AllowRefreshTokenFlow,
                Permissions.Endpoints.Token);

            SetPermission(
                command.AllowAuthorizationCodeFlow,
                Permissions.ResponseTypes.Code);

            SetPermission(
                command.AllowImplicitFlow,
                Permissions.ResponseTypes.IdToken);

            SetPermission(
                command.AllowImplicitFlow && IsPublic(command),
                Permissions.ResponseTypes.IdTokenToken,
                Permissions.ResponseTypes.Token);

            SetPermission(
                command.AllowHybridFlow,
                Permissions.ResponseTypes.CodeIdToken);

            SetPermission(
                command.AllowHybridFlow && IsPublic(command),
                Permissions.ResponseTypes.CodeIdTokenToken,
                Permissions.ResponseTypes.CodeToken);

            RemovePermissionsOfType(Permissions.Prefixes.Scope);

            if (command.AllowedScopes != null)
            {
                descriptor.Permissions.UnionWith(
                    command.AllowedScopes
                    .GetDelimitedElements()
                    .Select(s => Permissions.Prefixes.Scope + s));
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
            descriptor.Permissions.RemoveWhere(p => p.StartsWith(permissionType));
        }

        private string GetClientSecret(string currentClientSecret)
        {
            if (IsConfidential(command))
            {
                return currentClientSecret is null
                    ? Guid.NewGuid().ToString()
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
