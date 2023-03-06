// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.Email
{
    public class EmailOptions
    {
        public const string Section = "Email";

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool RequiresAuthentication { get; set; }

        public string Security { get; set; }

        public string TemplatesPath { get; set; } = Path.Combine("Templates", "Emails");

        public EmailTemplateOptions Template { get; } = new EmailTemplateOptions();
    }
}
