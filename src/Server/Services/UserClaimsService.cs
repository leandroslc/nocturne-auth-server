// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Extensions;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Server.Services
{
    public class UserClaimsService : IUserClaimsService
    {
        private const string RoleClaim = Claims.Role;
        private const string PermissionClaim = "permission";

        private readonly GetUserAccessHandler getUserAccessHandler;
        private readonly IOpenIddictScopeManager scopeManager;

        public UserClaimsService(
            GetUserAccessHandler getUserAccessHandler,
            IOpenIddictScopeManager scopeManager)
        {
            this.getUserAccessHandler = getUserAccessHandler;
            this.scopeManager = scopeManager;
        }

        public async Task AddClaimsToPrincipalAsync(
            ClaimsPrincipal principal,
            OpenIddictRequest request)
        {
            AddScopes(principal, request);

            await AddResourcesAsync(principal, principal.GetScopes());

            await AddPermissionsAndRolesAsync(principal, request);
        }

        public Task AddClaimsDestinationsAsync(ClaimsPrincipal principal)
        {
            // By default claims are not included in the access or identity tokens.
            // To allow them to be serialized, each claim must have a destination
            // attached.
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return Task.CompletedTask;
        }

        private static void AddScopes(ClaimsPrincipal principal, OpenIddictRequest request)
        {
            // In this case all granted scopes match the requested scopes, so all the
            // scopes are required by the application.
            principal.SetScopes(request.GetScopes());
        }

        private async Task AddResourcesAsync(
            ClaimsPrincipal principal,
            ImmutableArray<string> scopes)
        {
            var resources = await scopeManager.ListResourcesAsync(scopes).ToListAsync();

            principal.SetResources(resources);
        }

        private async Task AddPermissionsAndRolesAsync(
            ClaimsPrincipal principal,
            OpenIddictRequest request)
        {
            var command = new GetUserAccessCommand
            {
                ClientId = request.ClientId,
                User = principal,
            };

            var result = await getUserAccessHandler.HandleAsync(command);

            if (result.IsSuccess)
            {
                principal.SetClaims(RoleClaim, result.Value.Roles.ToImmutableArray());
                principal.SetClaims(PermissionClaim, result.Value.Permissions.ToImmutableArray());
            }
        }

        private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            switch (claim.Type)
            {
                // Secret values
                case "AspNet.Identity.SecurityStamp":
                    break;

                // Adds to the id_token if the corresponding scope was granted
                case Claims.Name when principal.HasScope(Scopes.Profile):
                case Claims.Email when principal.HasScope(Scopes.Email):
                case RoleClaim when principal.HasScope(Scopes.Roles):
                case PermissionClaim when principal.HasScope(Scopes.Roles):
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    break;

                default:
                    yield return Destinations.AccessToken;
                    break;
            }
        }
    }
}
