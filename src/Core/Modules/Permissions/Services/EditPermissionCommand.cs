// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class EditPermissionCommand : ManagePermissionCommand
    {
        public EditPermissionCommand()
        {
        }

        public EditPermissionCommand(Permission permission)
        {
            Id = permission.Id;
            Name = permission.Name;
            Description = permission.Description;
            ApplicationId = permission.ApplicationId;
        }

        public long? Id { get; set; }
    }
}
