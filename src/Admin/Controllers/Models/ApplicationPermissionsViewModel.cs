// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using Nocturne.Auth.Core.Modules.Permissions.Services;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class ApplicationPermissionsViewModel
    {
        public ApplicationPermissionsViewModel(
            string applicationId,
            IReadOnlyCollection<ListApplicationPermissionsItem> permissions)
        {
            ApplicationId = applicationId;
            Permissions = permissions;
        }

        public string ApplicationId { get; private set; }

        public IReadOnlyCollection<ListApplicationPermissionsItem> Permissions { get; private set; }
    }
}
