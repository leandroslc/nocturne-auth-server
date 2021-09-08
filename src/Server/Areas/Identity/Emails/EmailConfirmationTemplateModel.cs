// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Services.Email;

namespace Nocturne.Auth.Server.Areas.Identity.Emails
{
    public class EmailConfirmationTemplateModel : EmailTemplateModel
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public string CallbackUrl { get; set; }

        public string ConfirmButtonText { get; set; }
    }
}
