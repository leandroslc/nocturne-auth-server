// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        Task<IReadOnlyCollection<TResult>> QueryByApplication<TResult>(
            string applicationId,
            Func<IQueryable<Role>, IQueryable<TResult>> query);
    }
}
