using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Services;
using Nocturne.Auth.Core.OpenIddict.Applications.Commands;
using Nocturne.Auth.Core.OpenIddict.Applications.Handlers;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    [Area("Applications")]
    [Route("applications")]
    public class ApplicationsController : Controller
    {
        private readonly CreateApplicationHandler createApplicationHandler;
        private readonly EditApplicationHandler editApplicationHandler;
        private readonly ViewApplicationHandler viewApplicationHandler;

        public ApplicationsController(
            CreateApplicationHandler createApplicationHandler,
            EditApplicationHandler editApplicationHandler,
            ViewApplicationHandler viewApplicationHandler)
        {
            this.createApplicationHandler = createApplicationHandler;
            this.editApplicationHandler = editApplicationHandler;
            this.viewApplicationHandler = viewApplicationHandler;
        }

        [HttpGet("", Name = RouteNames.ApplicationsHome)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("new", Name = RouteNames.ApplicationsNew)]
        public async Task<IActionResult> Create()
        {
            var command = await createApplicationHandler.CreateCommandAsync();

            return View(command);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApplicationCommand command)
        {
            await ModelState.AddErrorsFromValidationAsync(
                createApplicationHandler.ValidateAsync(command));

            if (ModelState.IsValid is false)
            {
                return View(command);
            }

            var applicationId = await createApplicationHandler.HandleAsync(command);

            return RedirectToDetails(applicationId);
        }

        [HttpGet("{id}", Name = RouteNames.ApplicationsView)]
        public async Task<IActionResult> Details(ViewApplicationCommand command)
        {
            if (await viewApplicationHandler.ExistsAsync(command) is false)
            {
                return NotFound();
            }

            var result = await viewApplicationHandler.HandleAsync(command);

            return View(result);
        }

        [HttpGet("{id}/edit", Name = RouteNames.ApplicationsEdit)]
        public async Task<IActionResult> Edit(string id)
        {
            if (await editApplicationHandler.ExistsAsync(id) is false)
            {
                return NotFound();
            }

            var command = await editApplicationHandler.CreateCommandAsync(id);

            return View(command);
        }

        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditApplicationCommand command)
        {
            if (await editApplicationHandler.ExistsAsync(command.Id) is false)
            {
                return NotFound();
            }

            await ModelState.AddErrorsFromValidationAsync(
                editApplicationHandler.ValidateAsync(command));

            if (ModelState.IsValid is false)
            {
                return View(command);
            }

            await editApplicationHandler.HandleAsync(command);

            return RedirectToDetails(command.Id);
        }

        private IActionResult RedirectToDetails(string id)
        {
            return RedirectToRoute(RouteNames.ApplicationsView, new { id });
        }
    }
}