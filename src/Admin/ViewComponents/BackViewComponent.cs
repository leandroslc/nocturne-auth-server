using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.ViewComponents
{
    public class BackViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string url, string title)
        {
            var model = new BackViewComponentModel
            {
                Url = url,
                Title = title,
            };

            return View(model);
        }
    }
}
