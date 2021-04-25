using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Permissions.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("applications/{applicationId}/permissions")]
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

        public IActionResult ViewWithErrors(object model)
        {
            Response.StatusCode = 400;

            return View(model);
        }
    }
}
