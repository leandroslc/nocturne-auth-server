// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.OpenIddict.Managers
{
    public interface ICustomOpenIddictApplicationManager
    {
        ValueTask<string> GetUnprotectedClientSecretAsync(
            object application,
            CancellationToken cancellationToken = default);
    }
}
