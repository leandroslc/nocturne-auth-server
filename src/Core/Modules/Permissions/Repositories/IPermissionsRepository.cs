// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Modules.Permissions.Repositories
{
    public interface IPermissionsRepository
    {
        Task<bool> Exists(long id);

        Task<bool> HasDuplicated(Permission permission);

        Task<Permission> GetById(long id);

        Task DeleteAsync(Permission permission);

        Task InsertAsync(Permission permission);

        Task UpdateAsync(Permission permission);

        Task<IReadOnlyCollection<TResult>> QueryByApplication<TResult>(
            string applicationId,
            Func<IQueryable<Permission>, IQueryable<TResult>> query);

        Task<IReadOnlyCollection<TResult>> QueryByRole<TResult>(
            long roleId,
            Func<IQueryable<Permission>, IQueryable<TResult>> query);
    }
}
