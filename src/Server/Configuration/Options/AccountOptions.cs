// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Server.Configuration.Options
{
    public class AccountOptions
    {
        public const string Section = "Account";

        public bool EnableExternalAccount { get; set; }

        public bool EnableAccountDeletion { get; set; }

        public bool ShowRememberLogin { get; set; }

        public string PersonalDataFileName { get; set; } = "personal-data.json";
    }
}
