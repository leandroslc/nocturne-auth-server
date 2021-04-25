using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Modules.Permissions.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly AuthorizationDbContext context;

        public PermissionsRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public Task<bool> Exists(long id)
        {
            return context
                .Set<Permission>()
                .AnyAsync(p => p.Id == id);
        }

        public Task<Permission> GetById(long id)
        {
            return context
                .Set<Permission>()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<bool> HasDuplicated(Permission permission)
        {
            return context
                .Set<Permission>()
                .AnyAsync(p => p.Name == permission.Name && p.Id != permission.Id);
        }

        public async Task DeleteAsync(Permission permission)
        {
            context.Remove(permission);

            await context.SaveChangesAsync();
        }

        public async Task InsertAsync(Permission permission)
        {
            context.Set<Permission>().Add(permission);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            permission.UpdateConcurrencyToken();

            context.Update(permission);

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<TResult>> QueryByApplication<TResult>(
            string applicationId,
            Func<IQueryable<Permission>, IQueryable<TResult>> query)
        {
            var permissions = context
                .Set<Permission>()
                .Where(p => p.ApplicationId == applicationId);

            return await query(permissions).ToListAsync();
        }
    }
}
