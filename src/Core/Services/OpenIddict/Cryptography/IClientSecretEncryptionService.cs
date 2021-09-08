// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.OpenIddict.Cryptography
{
    public interface IClientSecretEncryptionService
    {
        string Encrypt(string plainText);

        string Decrypt(string cypherText);
    }
}
