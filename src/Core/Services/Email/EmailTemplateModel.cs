// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailTemplateModel
    {
        public string ApplicationName { get; set; }

        public string CompanyInfo { get; set; }

        public Uri CompanyLogoUrl { get; set; }

        public string CompanyName { get; set; }

        public string Title { get; set; }
    }
}
