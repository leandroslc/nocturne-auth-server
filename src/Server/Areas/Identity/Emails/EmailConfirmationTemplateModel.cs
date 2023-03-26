// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Services.Email;

namespace Nocturne.Auth.Server.Areas.Identity.Emails
{
    public class EmailConfirmationTemplateModel : EmailTemplateModel
    {
        public string UserName { get; set; }

        public Uri CallbackUrl { get; set; }
    }
}
