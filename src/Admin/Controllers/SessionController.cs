using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using AuthOptions = Nocturne.Auth.Admin.Configuration.Options.AuthorizationOptions;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("session")]
    [Authorize]
    public class SessionController : Controller
    {
        [HttpGet("account", Name = RouteNames.SessionAccount)]
        public IActionResult Account(
            [FromServices]AuthOptions options)
        {
            return Redirect(options.Authority);
        }

        [HttpGet("logout", Name = RouteNames.SessionLogout)]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(ApplicationConstants.AuthenticationScheme);
            await HttpContext.SignOutAsync(ApplicationConstants.AuthenticationChallengeScheme);
        }
    }
}
