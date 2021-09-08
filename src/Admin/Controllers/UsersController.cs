// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;
using Nocturne.Auth.Admin.Controllers.Models;
using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Results;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("users")]
    [Authorize(Policy = Policies.ManageUsers)]
    public class UsersController : CustomController
    {
        [HttpGet("", Name = RouteNames.UsersHome)]
        public async Task<IActionResult> Index(
            [FromServices] ListUsersHandler handler,
            ListUsersCommand command)
        {
            var results = await handler.HandleAsync(command);

            var model = new UserIndexViewModel
            {
                Users = results,
                Query = command,
            };

            return View(model);
        }

        [HttpGet("{id}", Name = RouteNames.UsersView)]
        public async Task<IActionResult> Details(
            [FromServices] ViewUserHandler handler,
            ViewUserCommand command)
        {
            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return View(result.User);
            }

            if (result.IsNotFound)
            {
                return NotFound();
            }

            throw new ResultNotHandledException(result);
        }
    }
}
