// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Roles.Services;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class ApplicationRolesViewModel
    {
        public ApplicationRolesViewModel(
            string applicationId,
            IReadOnlyCollection<ListApplicationRolesItem> roles)
        {
            ApplicationId = applicationId;
            Roles = roles;
        }

        public string ApplicationId { get; private set; }

        public IReadOnlyCollection<ListApplicationRolesItem> Roles { get; private set; }
    }
}
