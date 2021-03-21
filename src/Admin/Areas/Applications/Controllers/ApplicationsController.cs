using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Services;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Handlers;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    [Area("Applications")]
    [Route("applications")]
    public class ApplicationsController : Controller
    {
        private readonly IOpenIddictScopeManager scopeManager;
        private readonly CreateApplicationHandler createApplicationHandler;

        public ApplicationsController(
            IOpenIddictScopeManager scopeManager,
            CreateApplicationHandler createApplicationHandler)
        {
            this.scopeManager = scopeManager;
            this.createApplicationHandler = createApplicationHandler;
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
            var validationResult = createApplicationHandler.ValidateAsync(command);

            await ModelState.AddErrorsFromValidationAsync(validationResult);

            if (ModelState.IsValid is false)
            {
                return View(command);
            }

            await createApplicationHandler.HandleAsync(command);

            return RedirectToAction("Index");
        }

        private ValueTask<string> GetScopeNameAsync(object scope)
        {
            return scopeManager.GetNameAsync(scope);
        }
    }
}
