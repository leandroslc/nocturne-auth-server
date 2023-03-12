// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessReturn
    {
        public GetUserAccessReturn(
            IReadOnlyCollection<string> roles)
        {
            Roles = roles;
        }

        public IReadOnlyCollection<string> Roles { get; }
    }
}
