// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Server.Services
{
    public interface IUserClaimsService
    {
        Task AddClaimsDestinationsAsync(ClaimsPrincipal principal);

        Task AddClaimsToPrincipalAsync(ClaimsPrincipal principal, OpenIddictRequest request);
    }
}
