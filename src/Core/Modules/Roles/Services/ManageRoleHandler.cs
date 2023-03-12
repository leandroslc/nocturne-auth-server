// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.Localization;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public abstract class ManageRoleHandler
    {
        protected ManageRoleHandler(
            IStringLocalizer<ManageRoleHandler> localizer,
            IRolesRepository rolesRepository)
        {
            Localizer = localizer;
            RolesRepository = rolesRepository;
        }

        protected IStringLocalizer Localizer { get; }

        protected IRolesRepository RolesRepository { get; }
    }
}
