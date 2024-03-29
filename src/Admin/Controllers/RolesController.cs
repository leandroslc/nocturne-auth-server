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
    [Route("roles")]
    [Authorize(Policy = Policies.ManageApplications)]
    public class RolesController : CustomController
    {
        [HttpGet("", Name = RouteNames.RolesHome)]
        public async Task<IActionResult> Index(
            [FromServices] ListRolesHandler handler,
            ListRolesCommand command)
        {
            var result = await handler.HandleAsync(command);

            var model = new RolesIndexViewModel(result.Roles, command);

            return View(model);
        }

        [HttpGet("new", Name = RouteNames.RolesNew)]
        public IActionResult Create()
        {
            var command = new CreateRoleCommand();

            return View(command);
        }

        [HttpPost("new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [FromServices] CreateRoleHandler handler,
            CreateRoleCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return RedirectToDetails(result.RoleId);
            }

            if (result.IsFailure || result.IsDuplicated)
            {
                AddError(result.ErrorDescription);

                return ViewWithErrors(command);
            }

            throw new ResultNotHandledException(result);
        }

        [HttpGet("{id}/edit", Name = RouteNames.RolesEdit)]
        public async Task<IActionResult> Edit(
            [FromServices] EditRoleHandler handler,
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
            [FromServices] EditRoleHandler handler,
            EditRoleCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return RedirectToDetails(result.RoleId);
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

        [HttpGet("{id}", Name = RouteNames.RolesView)]
        public async Task<IActionResult> Details(
            [FromServices] ViewRoleHandler handler,
            ViewRoleCommand command)
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

        [HttpGet("{id}/delete", Name = RouteNames.RolesDelete)]
        public async Task<IActionResult> Delete(
            [FromServices] DeleteRoleHandler handler,
            long? id)
        {
            var command = await handler.CreateCommandAsync(id);

            if (command is null)
            {
                return NotFound();
            }

            return View(command);
        }

        [HttpPost("{id}/delete", Name = RouteNames.RolesDelete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            [FromServices] DeleteRoleHandler handler,
            DeleteRoleCommand command)
        {
            if (ModelState.IsValid is false)
            {
                return ViewWithErrors(command);
            }

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return RedirectToRoute(RouteNames.RolesHome);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }

        private IActionResult RedirectToDetails(long roleId)
        {
            return RedirectToRoute(
                RouteNames.RolesView,
                new
                {
                    id = roleId,
                });
        }

        private IActionResult ViewWithErrors(object model)
        {
            Response.StatusCode = 400;

            return View(model);
        }
    }
}
