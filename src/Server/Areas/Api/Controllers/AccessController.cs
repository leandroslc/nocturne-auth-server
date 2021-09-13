// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Core.Modules.Users.Services;
using Nocturne.Auth.Core.Shared.Results;
using OpenIddict.Validation.AspNetCore;

namespace Nocturne.Auth.Server.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/access")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class AccessController : ControllerBase
    {
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetByClient(
            [FromServices]GetUserAccessHandler handler,
            string clientId)
        {
            var command = new GetUserAccessCommand
            {
                ClientId = clientId,
                User = User,
            };

            var result = await handler.HandleAsync(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.IsNotFound)
            {
                return NotFound(result.ErrorDescription);
            }

            throw new ResultNotHandledException(result);
        }
    }
}
