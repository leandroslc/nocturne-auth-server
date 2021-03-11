using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
