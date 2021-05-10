using System.Linq;

namespace Nocturne.Auth.Core.Modules.Roles.Repositories
{
    public interface IUserRolesRepository
    {
        IQueryable<Role> QueryByUser(long id);
    }
}
