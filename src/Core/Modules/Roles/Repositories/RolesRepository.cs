using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AuthorizationDbContext context;

        public RolesRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public Task<bool> Exists(long id)
        {
            return context
                .Set<Role>()
                .AnyAsync(p => p.Id == id);
        }

        public Task<Role> GetById(long id)
        {
            return context
                .Set<Role>()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<bool> HasDuplicated(Role role)
        {
            return context
                .Set<Role>()
                .AnyAsync(p =>
                    p.Name == role.Name &&
                    p.ApplicationId == role.ApplicationId &&
                    p.Id != role.Id);
        }

        public async Task DeleteAsync(Role role)
        {
            context.Remove(role);

            await context.SaveChangesAsync();
        }

        public async Task InsertAsync(Role role)
        {
            context.Set<Role>().Add(role);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            role.UpdateConcurrencyToken();

            context.Update(role);

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<TResult>> QueryByApplication<TResult>(
            string applicationId,
            Func<IQueryable<Role>, IQueryable<TResult>> query)
        {
            var roles = context
                .Set<Role>()
                .Where(p => p.ApplicationId == applicationId);

            return await query(roles).ToListAsync();
        }
    }
}
