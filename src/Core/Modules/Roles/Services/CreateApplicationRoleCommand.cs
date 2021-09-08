// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class CreateApplicationRoleCommand : ManageApplicationRoleCommand
    {
        public CreateApplicationRoleCommand()
        {
        }

        public CreateApplicationRoleCommand(RoleApplication application)
            : base(application)
        {
        }
    }
}
