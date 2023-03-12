// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class EditRoleCommand : ManageRoleCommand
    {
        public EditRoleCommand()
        {
        }

        public EditRoleCommand(Role role)
            : base()
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public long? Id { get; set; }
    }
}
