// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Configuration.Models
{
    public class ApplicationDataProtectionOptions
    {
        public const string Section = "DataProtection";

        public string EncryptionCertificateThumbprint { get; set; }
    }
}
