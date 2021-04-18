using Microsoft.AspNetCore.Mvc;
using Result = Nocturne.Auth.Core.Shared.Results.Result;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    public class CustomController : Controller
    {
        protected ResultToActionResultBuilder GetResultBuilder(Result result)
        {
            return new ResultToActionResultBuilder(this, result);
        }
    }
}
