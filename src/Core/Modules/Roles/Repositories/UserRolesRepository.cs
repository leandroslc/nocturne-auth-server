using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly AuthorizationDbContext context;

        public UserRolesRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public async Task AssignRolesAsync(ApplicationUser user, IEnumerable<Role> roles)
        {
            var userRoles = roles.Select(role => new UserRole(user, role));

            context.AddRange(userRoles);

            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Role>> GetUnassignedRolesAsync(
            ApplicationUser user, IEnumerable<long> roleIds)
        {
            return await (
                from role in context.Set<Role>()
                join userRoleJoin in context.Set<UserRole>() on role.Id equals userRoleJoin.RoleId into j
                from userRole in j.DefaultIfEmpty()
                where roleIds.Contains(role.Id) && userRole == null
                select role)
                .ToListAsync();
        }

        public IQueryable<Role> QueryByUser(long userId)
        {
            return context
                .Set<UserRole>()
                .Where(p => p.UserId == userId)
                .Select(p => p.Role);
        }

        public async Task UnassignRoleAsync(ApplicationUser user, Role role)
        {
            var userRole = await context
                .Set<UserRole>()
                .FirstOrDefaultAsync(p => p.RoleId == role.Id && p.UserId == user.Id);

            if (userRole is null)
            {
                return;
            }

            context.Remove(userRole);

            await context.SaveChangesAsync();
        }
    }
}
