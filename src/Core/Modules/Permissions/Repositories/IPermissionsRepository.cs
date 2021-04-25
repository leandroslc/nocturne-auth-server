using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocturne.Auth.Core.Modules.Permissions.Repositories
{
    public interface IPermissionsRepository
    {
        Task<bool> Exists(long id);

        Task<bool> HasDuplicated(Permission permission);

        Task<Permission> GetById(long id);

        Task InsertAsync(Permission permission);

        Task UpdateAsync(Permission permission);

        Task<IReadOnlyCollection<TResult>> QueryByApplication<TResult>(
            string applicationId,
            Func<IQueryable<Permission>, IQueryable<TResult>> query);
    }
}
