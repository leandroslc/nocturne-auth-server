// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignPermissionsToRoleCommand
    {
        public AssignPermissionsToRoleCommand()
        {
        }

        public AssignPermissionsToRoleCommand(
            string currentApplicationId,
            IReadOnlyCollection<RoleApplication> availableApplications)
        {
            CurrentApplicationId = currentApplicationId;
            AvailableApplications = availableApplications;
        }

        public long? RoleId { get; set; }

        public IReadOnlyCollection<AssignPermissionsToRolePermission> Permissions { get; set; }

        public string CurrentApplicationId { get; private set; }

        public IReadOnlyCollection<RoleApplication> AvailableApplications { get; private set; }
    }
}
