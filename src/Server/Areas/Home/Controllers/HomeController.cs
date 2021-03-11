using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Server.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
