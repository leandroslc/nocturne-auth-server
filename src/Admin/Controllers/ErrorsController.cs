using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("error")]
    public class ErrorsController : Controller
    {
        [Route("denied")]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
