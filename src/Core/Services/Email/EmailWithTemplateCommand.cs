// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailWithTemplateCommand
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string TemplateName { get; set; }

        public EmailTemplateModel TemplateModel { get; set; }
    }
}
