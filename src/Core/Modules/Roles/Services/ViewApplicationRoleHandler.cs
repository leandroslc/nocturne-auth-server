using System.Threading.Tasks;
using Nocturne.Auth.Core.Modules.Roles.Repositories;

namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewApplicationRoleHandler
    {
        private readonly IRolesRepository rolesRepository;

        public ViewApplicationRoleHandler(
            IRolesRepository rolesRepository)
        {
            this.rolesRepository = rolesRepository;
        }

        public async Task<ViewApplicationRoleResult> HandleAsync(
            ViewApplicationRoleCommand command)
        {
            var role = await GetRoleAsync(command.Id);

            if (role is null)
            {
                return ViewApplicationRoleResult.NotFound();
            }

            return ViewApplicationRoleResult.Success(role);
        }

        private async Task<Role> GetRoleAsync(long? id)
        {
            if (id.HasValue is false)
            {
                return null;
            }

            return await rolesRepository.GetById(id.Value);
        }
    }
}
