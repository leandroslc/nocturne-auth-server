// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class EditApplicationCommand : ManageApplicationCommand
    {
        public string Id { get; set; }
    }
}
