using System.Linq;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly AuthorizationDbContext context;

        public UserRolesRepository(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Role> QueryByUser(long userId)
        {
            return context
                .Set<UserRole>()
                .Where(p => p.UserId == userId)
                .Select(p => p.Role);
        }
    }
}
