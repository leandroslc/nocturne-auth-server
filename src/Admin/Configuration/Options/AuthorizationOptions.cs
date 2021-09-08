// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;

namespace Nocturne.Auth.Admin.Configuration.Options
{
    public sealed class AuthorizationOptions
    {
        public const string Section = "Authorization";

        public AuthorizationOptions()
        {
            Scopes = new HashSet<string>();
        }

        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public ICollection<string> Scopes { get; set; }

        public bool RequireHttps { get; set; } = true;
    }
}
