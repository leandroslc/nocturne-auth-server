// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.OpenIddict.Services
{
    public interface IClientBuilderService
    {
        string GenerateClientId();

        string GenerateClientSecret();
    }
}
