using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("roles/{roleId}/permissions")]
    [Authorize(Policy = Policies.ManageApplications)]
    public class RolePermissionsController : CustomController
    {
        [HttpGet("", Name = RouteNames.RolePermissionsHome)]
        public async Task<IActionResult> List(
            [FromServices]ListRolePermissionsHandler handler,
            ListRolePermissionsCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var model = new RolePermissionsViewModel(command.RoleId.Value, result.Permissions);

                return View(model);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("add", Name = RouteNames.RolePermissionsAdd)]
        public async Task<IActionResult> Add(
            [FromServices]AssignPermissionsToRoleHandler handler,
            long? roleId,
            string applicationId)
        {
            if (await handler.RoleExistsAsync(roleId) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(roleId, applicationId);

            return View(command);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(
            [FromServices]AssignPermissionsToRoleHandler handler,
            AssignPermissionsToRoleCommand command)
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

            if (result.IsFailure)
            {
                AddError(result.ErrorDescription);

                return ViewWithErrors(command);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorDescription);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("{permissionId}/remove", Name = RouteNames.RolePermissionsRemove)]
        public IActionResult Remove(
            long? roleId,
            long? permissionId)
        {
            var command = new UnassignPermissionFromRoleCommand
            {
                RoleId = roleId,
                PermissionId = permissionId,
            };

            return View(command);
        }

        [HttpPost("{permissionId}/remove")]
        public async Task<IActionResult> Remove(
            [FromServices]UnassignPermissionFromRoleHandler handler,
            UnassignPermissionFromRoleCommand command)
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
                return NotFound(result.ErrorDescription);
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
