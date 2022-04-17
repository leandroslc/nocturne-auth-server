// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Configuration.Options
{
    public class LocalizationOptions
    {
        public const string Section = "Localization";

        public string DefaultCulture { get; set; } = "en";
    }
}
