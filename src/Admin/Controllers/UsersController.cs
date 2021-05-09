using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Users.Services;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("users")]
    public class UsersController : CustomController
    {
        [HttpGet("", Name = RouteNames.UsersHome)]
        public async Task<IActionResult> Index(
            [FromServices] ListUsersHandler handler,
            ListUsersCommand command)
        {
            var results = await handler.HandleAsync(command);

            var model = new UserIndexViewModel
            {
                Users = results,
                Query = command,
            };

            return View(model);
        }
    }
}
