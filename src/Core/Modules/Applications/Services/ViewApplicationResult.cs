// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ViewApplicationResult
    {
        public string Id { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string DisplayName { get; set; }

        public string ConsentType { get; set; }

        public string Type { get; set; }

        public ICollection<string> AllowedScopes { get; set; } = new HashSet<string>();

        public IReadOnlyCollection<string> RedirectUris { get; set; }

        public IReadOnlyCollection<string> PostLogoutRedirectUris { get; set; }

        public bool AllowPasswordFlow { get; set; }

        public bool AllowClientCredentialsFlow { get; set; }

        public bool AllowAuthorizationCodeFlow { get; set; }

        public bool AllowRefreshTokenFlow { get; set; }

        public bool AllowImplicitFlow { get; set; }

        public bool AllowLogoutEndpoint { get; set; }

        public bool HasAnyAllowedFlow =>
            AllowPasswordFlow ||
            AllowClientCredentialsFlow ||
            AllowAuthorizationCodeFlow ||
            AllowRefreshTokenFlow ||
            AllowImplicitFlow;
    }
}
