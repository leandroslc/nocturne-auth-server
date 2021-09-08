// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IUserRolesRepository
    {
        IQueryable<Role> QueryByUser(long userId);

        IQueryable<RolePermission> QueryRolePermissionsByUser(long userId);

        Task AssignRolesAsync(ApplicationUser user, IEnumerable<Role> roles);

        Task<IReadOnlyCollection<Role>> GetUnassignedRolesAsync(
            ApplicationUser user, IEnumerable<long> roleIds);

        Task UnassignRoleAsync(ApplicationUser user, Role role);
    }
}
