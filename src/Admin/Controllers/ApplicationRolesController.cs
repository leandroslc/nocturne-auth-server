// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Roles.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("application/{applicationId}/roles")]
    [Authorize(Policy = Policies.ManageApplications)]
    public class ApplicationRolesController : CustomController
    {
        [HttpGet("", Name = RouteNames.ApplicationRolesHome)]
        public async Task<IActionResult> List(
            [FromServices]ListApplicationRolesHandler handler,
            ListApplicationRolesCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                var model = new ApplicationRolesViewModel(command.ApplicationId, result.Roles);

                return View(model);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorMessage);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("new", Name = RouteNames.ApplicationRolesNew)]
        public async Task<IActionResult> Create(
            [FromServices]CreateApplicationRoleHandler handler,
            string applicationId)
        {
            if (await handler.ApplicationExists(applicationId) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(applicationId);

            return View(command);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromServices]CreateApplicationRoleHandler handler,
            CreateApplicationRoleCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return RedirectToDetails(result.RoleId, result.ApplicationId);
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

        [HttpGet("{id}/edit", Name = RouteNames.ApplicationRolesEdit)]
        public async Task<IActionResult> Edit(
            [FromServices]EditApplicationRoleHandler handler,
            long? id)
        {
            if (await handler.RoleExsits(id) is false)
            {
                return NotFound();
            }

            var command = await handler.CreateCommandAsync(id.Value);

            return View(command);
        }

        [HttpPost("{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromServices] EditApplicationRoleHandler handler,
            EditApplicationRoleCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return RedirectToDetails(result.RoleId, result.ApplicationId);
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

        [HttpGet("{id}", Name = RouteNames.ApplicationRolesView)]
        public async Task<IActionResult> Details(
            [FromServices]ViewApplicationRoleHandler handler,
            ViewApplicationRoleCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return View(result);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("{id}/delete", Name = RouteNames.ApplicationRolesDelete)]
        public async Task<IActionResult> Delete(
            [FromServices] DeleteApplicationRoleHandler handler,
            long? id)
        {
            var command = await handler.CreateCommandAsync(id);

            if (command is null)
            {
                return NotFound();
            }

            return View(command);
        }

        [HttpPost("{id}/delete", Name = RouteNames.ApplicationRolesDelete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            [FromServices]DeleteApplicationRoleHandler handler,
            DeleteApplicationRoleCommand command)
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

        private IActionResult RedirectToDetails(long roleId, string applicationId)
        {
            return RedirectToRoute(
                RouteNames.ApplicationRolesView,
                new
                {
                    id = roleId,
                    applicationId,
                });
        }

        private IActionResult ViewWithErrors(object model)
        {
            Response.StatusCode = 400;

            return View(model);
        }
    }
}
