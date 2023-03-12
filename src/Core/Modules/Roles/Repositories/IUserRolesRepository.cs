// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IUserRolesRepository
    {
        IQueryable<Role> QueryByUser(long userId);

        Task AssignRolesAsync(ApplicationUser user, IEnumerable<Role> roles);

        Task<IReadOnlyCollection<Role>> GetUnassignedRolesAsync(ApplicationUser user);

        Task UnassignRoleAsync(ApplicationUser user, Role role);
    }
}
