using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Areas.Applications.Models;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Core.Modules.Applications.Services;

namespace Nocturne.Auth.Admin.Areas.Applications.Controllers
{
    [Area("Applications")]
    [Route("applications")]
    public class ApplicationsController : CustomController
    {
        [HttpGet("", Name = RouteNames.ApplicationsHome)]
        public async Task<IActionResult> Index(
            [FromServices] ListApplicationsHandler handler,
            ListApplicationsCommand command)
        {
            var results = await handler.HandleAsync(command);

            var model = new ApplicationIndexViewModel
            {
                Applications = results,
                Query = command,
            };

            return View(model);
        }

        [HttpGet("new", Name = RouteNames.ApplicationsNew)]
        public async Task<IActionResult> Create(
            [FromServices] CreateApplicationHandler handler)
        {
            var command = await handler.CreateCommandAsync();

            return View(command);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromServices] CreateApplicationHandler handler,
            CreateApplicationCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return await Problems();
            }

            var (result, applicationId) = await handler.HandleAsync(command);

            return await GetResultBuilder(result)
                .Success(Success)
                .Problems(Problems)
                .BuildAsync();

            IActionResult Success() => RedirectToDetails(applicationId);

            async Task<IActionResult> Problems()
            {
                await handler.AddAvailableScopesAsync(command);

                return View(command);
            }
        }

        [HttpGet("{id}", Name = RouteNames.ApplicationsView)]
        public async Task<IActionResult> Details(
            [FromServices] ViewApplicationHandler handler,
            ViewApplicationCommand command)
        {
            if (await handler.ExistsAsync(command) is false)
            {
                return NotFound();
            }

            var result = await handler.HandleAsync(command);

            return View(result);
        }

        [HttpGet("{id}/edit", Name = RouteNames.ApplicationsEdit)]
        public async Task<IActionResult> Edit(
            [FromServices] EditApplicationHandler handler,
            string id)
        {
            if (await handler.ExistsAsync(id) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(id);

            return View(command);
        }

        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromServices] EditApplicationHandler handler,
            EditApplicationCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return await Problems();
            }

            var result = await handler.HandleAsync(command);

            return await GetResultBuilder(result)
                .Success(Success)
                .Problems(Problems)
                .BuildAsync();

            IActionResult Success() => RedirectToDetails(command.Id);

            async Task<IActionResult> Problems()
            {
                await handler.AddAvailableScopesAsync(command);

                return View(command);
            }
        }

        private IActionResult RedirectToDetails(string id)
        {
            return RedirectToRoute(RouteNames.ApplicationsView, new { id });
        }
    }
}
