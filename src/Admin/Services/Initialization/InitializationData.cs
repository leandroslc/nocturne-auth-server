// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using OpenIddict.Abstractions;

namespace Nocturne.Auth.Admin.Services.Initialization
{
    public class InitializationData
    {
        public const string Section = "Initialization";

        public ICollection<OpenIddictScopeDescriptor> Scopes { get; } = new List<OpenIddictScopeDescriptor>();

        public OpenIddictApplicationDescriptor AdminApplication { get; set; }

        public UserData AdminUser { get; set; }

        public class UserData
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }
    }
}
