using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("users/{userId}/roles")]
    public class UserRolesController : CustomController
    {
        [HttpGet("", Name = RouteNames.UserRolesHome)]
        public async Task<IActionResult> List(
            [FromServices]ListUserRolesHandler handler,
            ListUserRolesCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var model = new UserRolesViewModel(command.UserId.Value, result.Roles);

                return View(model);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("add", Name = RouteNames.UserRolesAdd)]
        public IActionResult Add()
        {
            return NoContent();
        }

        [HttpGet("{roleId}/remove", Name = RouteNames.UserRolesRemove)]
        public IActionResult Remove()
        {
            return NoContent();
        }
    }
}
