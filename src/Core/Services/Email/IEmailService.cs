// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(EmailWithTemplateCommand command);
    }
}
