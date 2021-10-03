// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Permissions;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IRolePermissionsRepository
    {
        Task AssignPermissionsAsync(Role role, IEnumerable<Permission> permissions);

        Task<IReadOnlyCollection<Permission>> GetUnassignedPermissionsAsync(Role role, string applicationId);

        Task UnassignPermissionAsync(Role role, Permission permission);
    }
}
