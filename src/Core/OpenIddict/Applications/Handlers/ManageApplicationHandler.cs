using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Extensions;
using Nocturne.Auth.Core.Helpers;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
            foreach (var result in UriHelper.Validate(
                command.RedirectUris,
                nameof(command.RedirectUris),
                InvalidUri))
            {
                yield return result;
            }

            foreach (var result in UriHelper.Validate(
                command.PostLogoutRedirectUris,
                nameof(command.PostLogoutRedirectUris),
                InvalidUri))
            {
                yield return result;
            }

            await Task.CompletedTask;
        }

        protected async Task<OpenIddictApplicationDescriptor> GenerateDescriptorAsync(
            TCommand command,
            object application = null)
        {
            var clientId = application is not null
                ? await ApplicationManager.GetClientIdAsync(application)
                : Guid.NewGuid().ToString();

            var currentClientSecret = application is not null
                ? await ApplicationManager.GetUnprotectedClientSecret(application)
                : null;

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = GetClientSecret(command, currentClientSecret),
                ConsentType = command.ConsentType,
                DisplayName = command.DisplayName,
                Type = command.Type,
            };

            AddUris(descriptor, command);

            AddPermissions(descriptor, command);

            return descriptor;
        }

        protected async Task AddAvailableScopesAsync(TCommand command)
        {
            var scopes = ScopeManager.ListAsync();

            await foreach (var scope in scopes)
            {
                command.AvailableScopes.Add(await ScopeManager.GetNameAsync(scope));
            }
        }

        private static void AddUris(
            OpenIddictApplicationDescriptor descriptor,
            TCommand command)
        {
            descriptor.PostLogoutRedirectUris.Clear();
            descriptor.PostLogoutRedirectUris.UnionWith(
                UriHelper.GetUris(command.PostLogoutRedirectUris));

            descriptor.RedirectUris.Clear();
            descriptor.RedirectUris.UnionWith(
                UriHelper.GetUris(command.RedirectUris));
        }

        private static string GetClientSecret(
            TCommand command,
            string currentClientSecret)
        {
            if (IsConfidential(command))
            {
                return currentClientSecret is null
                    ? Guid.NewGuid().ToString()
                    : currentClientSecret;
            }

            return null;
        }

        private static void AddPermissions(
            OpenIddictApplicationDescriptor descriptor,
            TCommand command)
        {
            SetPermission(
                descriptor,
                command.AllowLogoutEndpoint,
                Permissions.Endpoints.Logout);

            SetPermission(
                descriptor,
                command.AllowAuthorizationCodeFlow,
                Permissions.GrantTypes.AuthorizationCode);

            SetPermission(
                descriptor,
                command.AllowClientCredentialsFlow,
                Permissions.GrantTypes.ClientCredentials);

            SetPermission(
                descriptor,
                command.AllowImplicitFlow,
                Permissions.GrantTypes.Implicit);

            SetPermission(
                descriptor,
                command.AllowPasswordFlow,
                Permissions.GrantTypes.Password);

            SetPermission(
                descriptor,
                command.AllowRefreshTokenFlow,
                Permissions.GrantTypes.RefreshToken);

            SetPermission(
                descriptor,
                command.AllowAuthorizationCodeFlow || command.AllowImplicitFlow,
                Permissions.Endpoints.Authorization);

            SetPermission(
                descriptor,
                command.AllowAuthorizationCodeFlow ||
                command.AllowClientCredentialsFlow ||
                command.AllowPasswordFlow ||
                command.AllowRefreshTokenFlow,
                Permissions.Endpoints.Token);

            SetPermission(
                descriptor,
                command.AllowAuthorizationCodeFlow,
                Permissions.ResponseTypes.Code);

            SetPermission(
                descriptor,
                command.AllowImplicitFlow,
                Permissions.ResponseTypes.IdToken);

            SetPermission(
                descriptor,
                command.AllowImplicitFlow && IsPublic(command),
                Permissions.ResponseTypes.IdTokenToken,
                Permissions.ResponseTypes.Token);

            SetPermission(
                descriptor,
                command.AllowHybridFlow,
                Permissions.ResponseTypes.CodeIdToken);

            SetPermission(
                descriptor,
                command.AllowHybridFlow && IsPublic(command),
                Permissions.ResponseTypes.CodeIdTokenToken,
                Permissions.ResponseTypes.CodeToken);

            RemovePermissionsOfType(descriptor, Permissions.Prefixes.Scope);

            if (command.AllowedScopes != null)
            {
                descriptor.Permissions.UnionWith(
                    command.AllowedScopes
                    .GetDelimitedElements()
                    .Select(s => Permissions.Prefixes.Scope + s));
            }
        }

        private static void SetPermission(
            OpenIddictApplicationDescriptor descriptor,
            bool allow,
            params string[] permissions)
        {
            if (allow)
            {
                foreach (var permission in permissions)
                {
                    descriptor.Permissions.Add(permission);
                }
            }
            else
            {
                foreach (var permission in permissions)
                {
                    descriptor.Permissions.Remove(permission);
                }
            }
        }

        private static void RemovePermissionsOfType(
            OpenIddictApplicationDescriptor descriptor,
            string permissionType)
        {
            descriptor.Permissions.RemoveWhere(p => p.StartsWith(permissionType));
        }

        private static bool IsPublic(TCommand command)
        {
            return command.Type.IsEqualInvariant(ClientTypes.Public);
        }

        private static bool IsConfidential(TCommand command)
        {
            return command.Type.IsEqualInvariant(ClientTypes.Confidential);
        }

        private static string InvalidUri(string url)
        {
            return $"{url} is invalid";
        }
    }
}
