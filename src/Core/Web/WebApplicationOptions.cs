// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;

namespace Nocturne.Auth.Core.Web
{
    public class WebApplicationOptions
    {
        public const string Section = "Application";

        public string ApplicationName { get; set; }

        public string CompanyName { get; set; }

        public Uri PrivacyPolicyUrl { get; set; }

        public bool HasPrivacyPolicyUrl => PrivacyPolicyUrl != null;
    }
}
