// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class AssignRolesToUserCommand
    {
        public AssignRolesToUserCommand()
        {
        }

        public AssignRolesToUserCommand(
            string currentApplicationId,
            IReadOnlyCollection<RoleApplication> availableApplications)
        {
            ApplicationId = currentApplicationId;
            AvailableApplications = availableApplications;
        }

        public long? UserId { get; set; }

        public IReadOnlyCollection<AssignRolesToUserRole> Roles { get; set; }

        public string ApplicationId { get; set; }

        public IReadOnlyCollection<RoleApplication> AvailableApplications { get; private set; }
    }
}
