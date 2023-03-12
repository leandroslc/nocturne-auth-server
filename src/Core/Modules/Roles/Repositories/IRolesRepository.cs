// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IRolesRepository
    {
        Task<bool> Exists(long id);

        Task<bool> HasDuplicated(Role role);

        Task<Role> GetById(long id);

        Task DeleteAsync(Role role);

        Task InsertAsync(Role role);

        Task UpdateAsync(Role role);

        Task<IReadOnlyCollection<TResult>> Query<TResult>(
            Func<IQueryable<Role>, IQueryable<TResult>> query);
    }
}
