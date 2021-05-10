using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Users.Services
{
    public sealed class ViewUserHandler
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ViewUserHandler(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ViewUserResult> HandleAsync(ViewUserCommand command)
        {
            var user = await GetUserAsync(command.Id);

            if (user is null)
            {
                return ViewUserResult.NotFound();
            }

            var userData = new ViewUserItem(user);

            return ViewUserResult.Success(userData);
        }

        private async Task<ApplicationUser> GetUserAsync(long? id)
        {
            if (id.HasValue is false)
            {
                return null;
            }

            return await userManager.FindByIdAsync(id.Value.ToString());
        }
    }
}
