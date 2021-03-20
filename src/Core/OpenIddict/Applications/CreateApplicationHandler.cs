using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Helpers;
using Nocturne.Auth.Core.OpenIddict.Applications.Models;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Core.OpenIddict.Applications
{
    public class CreateApplicationHandler
    {
        private readonly IOpenIddictApplicationManager applicationManager;

        public CreateApplicationHandler(
            IOpenIddictApplicationManager applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public async Task Handle(CreateApplicationCommand command)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = GetClientSecret(command),
                ConsentType = command.ConsentType,
                DisplayName = command.DisplayName,
                Type = command.Type,
            };

            AddPermissions(descriptor, command);

            descriptor.PostLogoutRedirectUris.UnionWith(
                UriHelper.GetUris(command.PostLogoutRedirectUris));

            descriptor.RedirectUris.UnionWith(
                UriHelper.GetUris(command.RedirectUris));

            await applicationManager.CreateAsync(descriptor);
        }

        private static void AddPermissions(
            OpenIddictApplicationDescriptor descriptor,
            CreateApplicationCommand command)
        {
            if (command.AllowLogoutEndpoint)
            {
                descriptor.Permissions.Add(Permissions.Endpoints.Logout);
            }

            if (command.AllowAuthorizationCodeFlow)
            {
                descriptor.Permissions.Add(Permissions.GrantTypes.AuthorizationCode);
            }

            if (command.AllowClientCredentialsFlow)
            {
                descriptor.Permissions.Add(Permissions.GrantTypes.ClientCredentials);
            }

            if (command.AllowImplicitFlow)
            {
                descriptor.Permissions.Add(Permissions.GrantTypes.Implicit);
            }

            if (command.AllowPasswordFlow)
            {
                descriptor.Permissions.Add(Permissions.GrantTypes.Password);
            }

            if (command.AllowRefreshTokenFlow)
            {
                descriptor.Permissions.Add(Permissions.GrantTypes.RefreshToken);
            }

            if (command.AllowAuthorizationCodeFlow || command.AllowImplicitFlow)
            {
                descriptor.Permissions.Add(Permissions.Endpoints.Authorization);
            }

            if (command.AllowAuthorizationCodeFlow ||
                command.AllowClientCredentialsFlow ||
                command.AllowPasswordFlow ||
                command.AllowRefreshTokenFlow)
            {
                descriptor.Permissions.Add(Permissions.Endpoints.Token);
            }

            if (command.AllowAuthorizationCodeFlow)
            {
                descriptor.Permissions.Add(Permissions.ResponseTypes.Code);
            }

            if (command.AllowImplicitFlow)
            {
                descriptor.Permissions.Add(Permissions.ResponseTypes.IdToken);

                if (IsEqual(command.Type, ClientTypes.Public))
                {
                    descriptor.Permissions.Add(Permissions.ResponseTypes.IdTokenToken);
                    descriptor.Permissions.Add(Permissions.ResponseTypes.Token);
                }
            }

            if (command.AllowHybridFlow)
            {
                descriptor.Permissions.Add(Permissions.ResponseTypes.CodeIdToken);

                if (IsEqual(command.Type, ClientTypes.Public))
                {
                    descriptor.Permissions.Add(Permissions.ResponseTypes.CodeIdTokenToken);
                    descriptor.Permissions.Add(Permissions.ResponseTypes.CodeToken);
                }
            }

            if (command.AllowedScopes != null)
            {
                descriptor.Permissions.UnionWith(
                    GetElements(command.AllowedScopes)
                    .Select(s => Permissions.Prefixes.Scope + s));
            }
        }

        private static string GetClientSecret(CreateApplicationCommand command)
        {
            return IsEqual(command.Type, ClientTypes.Confidential)
                ? Guid.NewGuid().ToString()
                : null;
        }

        private static bool IsEqual(string value, string valueToCompare)
        {
            return string.Equals(value, valueToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        private static IEnumerable<string> GetElements(string value)
        {
            return value.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
