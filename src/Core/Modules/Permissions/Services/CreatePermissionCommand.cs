// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class CreatePermissionCommand : ManagePermissionCommand
    {
        public CreatePermissionCommand()
        {
        }

        public CreatePermissionCommand(string applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
