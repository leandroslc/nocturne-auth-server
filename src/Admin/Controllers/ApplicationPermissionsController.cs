using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Permissions.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("applications/{applicationId}/permissions")]
    [Authorize(Policy = Policies.ManageApplications)]
    public class ApplicationPermissionsController : CustomController
    {
        [HttpGet("", Name = RouteNames.ApplicationPermissionsHome)]
        public async Task<IActionResult> List(
            [FromServices] ListApplicationPermissionsHandler handler,
            ListApplicationPermissionsCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var model = new ApplicationPermissionsViewModel(command.ApplicationId, result.Permissions);

                return View(model);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("new", Name = RouteNames.ApplicationPermissionsNew)]
        public async Task<IActionResult> Create(
            [FromServices] CreatePermissionHandler handler,
            string applicationId)
        {
            if (await handler.ApplicationExists(applicationId) is false)
            {
                return NotFound();
            }

            var command = new CreatePermissionCommand(applicationId);

            return View(command);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Create(
            [FromServices] CreatePermissionHandler handler,
            CreatePermissionCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            if (result.IsFailure || result.IsDuplicated)
            {
                AddError(result.ErrorDescription);

                return ViewWithErrors(command);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("edit", Name = RouteNames.ApplicationPermissionsEdit)]
        public async Task<IActionResult> Edit(
            [FromServices] EditPermissionHandler handler,
            long? id)
        {
            if (id.HasValue is false || await handler.PermissionExsits(id.Value) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(id.Value);

            return View(command);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(
            [FromServices] EditPermissionHandler handler,
            long? id,
            EditPermissionCommand command)
        {
            command.Id = id;

            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            if (result.IsFailure || result.IsDuplicated)
            {
                AddError(result.ErrorDescription);

                return ViewWithErrors(command);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("{id}", Name = RouteNames.ApplicationPermissionsView)]
        public async Task<IActionResult> Details(
            [FromServices] ViewApplicationPermissionHandler handler,
            ViewApplicationPermissionCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return View(result.Permission);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("{id}/delete", Name = RouteNames.ApplicationPermissionsDelete)]
        public async Task<IActionResult> Delete(
            [FromServices] DeleteApplicationPermissionHandler handler,
            long? id)
        {
            var command = await handler.CreateCommandAsync(id);

            if (command is null)
            {
                return NotFound();
            }

            return View(command);
        }

        [HttpPost("{id}/delete")]
        public async Task<IActionResult> Delete(
            [FromServices] DeleteApplicationPermissionHandler handler,
            DeleteApplicationPermissionCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return Ok();
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        private IActionResult ViewWithErrors(object model)
        {
            Response.StatusCode = 400;

            return View(model);
        }
    }
}
