// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class GetUserAccessCommand
    {
        public string ClientId { get; set; }

        public ClaimsPrincipal User { get; set; }
    }
}
