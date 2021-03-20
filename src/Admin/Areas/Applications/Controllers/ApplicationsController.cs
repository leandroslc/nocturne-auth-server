using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Nocturne.Auth.Admin.Services;
using Nocturne.Auth.Core.OpenIddict.Applications;
using Nocturne.Auth.Core.OpenIddict.Applications.Models;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    [Area("Applications")]
    [Route("applications")]
    public class ApplicationsController : Controller
    {
        private readonly IOpenIddictScopeManager scopeManager;
        private readonly CreateApplicationHandler createApplicationHandler;
        private readonly CreateApplicationValidation createApplicationValidation;

        public ApplicationsController(
            IStringLocalizer<ApplicationsController> localizer,
            IOpenIddictScopeManager scopeManager,
            CreateApplicationHandler createApplicationHandler)
        {
            this.scopeManager = scopeManager;
            this.createApplicationHandler = createApplicationHandler;

            createApplicationValidation = new CreateApplicationValidation(localizer);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("new")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateApplicationCommand();

            var scopes = scopeManager.ListAsync();

            await model.AddScopesAsync(scopes, GetScopeNameAsync);

            return View(model);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create(CreateApplicationCommand command)
        {
            ModelState.AddErrorsFromValidation(
                createApplicationValidation.Handle(command));

            if (ModelState.IsValid == false)
            {
                return View(command);
            }

            await createApplicationHandler.Handle(command);

            return RedirectToAction("Index");
        }

        private ValueTask<string> GetScopeNameAsync(object scope)
        {
            return scopeManager.GetNameAsync(scope);
        }
    }
}
