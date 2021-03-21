using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public ApplicationsController(
            CreateApplicationHandler createApplicationHandler,
            EditApplicationHandler editApplicationHandler)
        {
            this.createApplicationHandler = createApplicationHandler;
            this.editApplicationHandler = editApplicationHandler;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("new")]
        public async Task<IActionResult> Create()
        {
            var command = await createApplicationHandler.CreateCommandAsync();

            return View(command);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create(CreateApplicationCommand command)
        {
            await ModelState.AddErrorsFromValidationAsync(
                createApplicationHandler.ValidateAsync(command));

            if (ModelState.IsValid is false)
            {
                return View(command);
            }

            await createApplicationHandler.HandleAsync(command);

            return RedirectToAction("Index");
        }

        [HttpGet("{id}/edit")]
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

            return RedirectToAction("Index");
        }
    }
}
