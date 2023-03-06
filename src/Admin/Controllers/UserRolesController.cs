// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("users/{userId}/roles")]
    [Authorize(Policy = Policies.ManageUserRoles)]
    public class UserRolesController : CustomController
    {
        [HttpGet("", Name = RouteNames.UserRolesHome)]
        public async Task<IActionResult> List(
            [FromServices] ListUserRolesHandler handler,
            ListUserRolesCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var model = new UserRolesViewModel(command.UserId.Value, result.Roles);

                return View(model);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("add", Name = RouteNames.UserRolesAdd)]
        public async Task<IActionResult> Add(
            [FromServices] AssignRolesToUserHandler handler,
            long? userId,
            string applicationId)
        {
            if (await handler.UserExistsAsync(userId) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(userId, applicationId);

            return View(command);
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [FromServices] AssignRolesToUserHandler handler,
            AssignRolesToUserCommand command)
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

        [HttpGet("{roleId}/remove", Name = RouteNames.UserRolesRemove)]
        public IActionResult Remove(long? userId, long? roleId)
        {
            var command = new UnassignRoleFromUserCommand
            {
                UserId = userId,
                RoleId = roleId,
            };

            return View(command);
        }

        [HttpPost("{roleId}/remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(
            [FromServices] UnassignRoleFromUserHandler handler,
            UnassignRoleFromUserCommand command)
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
