// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("error")]
    public class ErrorsController : Controller
    {
        [HttpGet("denied")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet("unexpected")]
        public IActionResult InternalServerError()
        {
            return View();
        }

        [HttpGet("remote-auth")]
        public IActionResult RemoteAuthenticationError()
        {
            return View();
        }
    }
}
