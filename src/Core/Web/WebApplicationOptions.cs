// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Web
{
    public class WebApplicationOptions
    {
        public const string Section = "Application";

        public string ApplicationName { get; set; }

        public string CompanyName { get; set; }

        public string PrivacyPolicyUrl { get; set; }

        public bool HasPrivacyPolicyUrl => !string.IsNullOrWhiteSpace(PrivacyPolicyUrl);
    }
}
