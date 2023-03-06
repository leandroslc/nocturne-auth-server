// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

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
            ApplicationId = currentApplicationId;
            AvailableApplications = availableApplications;
        }

        public long? RoleId { get; set; }

        public IReadOnlyCollection<AssignPermissionsToRolePermission> Permissions { get; set; }

        public string ApplicationId { get; set; }

        public IReadOnlyCollection<RoleApplication> AvailableApplications { get; private set; }
    }
}
