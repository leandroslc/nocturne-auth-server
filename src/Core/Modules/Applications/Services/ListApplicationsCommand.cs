// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Core.Modules.Applications.Services
{
    public class ListApplicationsCommand : PagedCommand<ListApplicationsResult>
    {
        public string Name { get; set; }
    }
}
