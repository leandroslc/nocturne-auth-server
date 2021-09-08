// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("error")]
    public class ErrorsController : Controller
    {
        [Route("denied")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Route("unexpected")]
        public IActionResult InternalServerError()
        {
            return View();
        }

        [Route("remote-auth")]
        public IActionResult RemoteAuthenticationError()
        {
            return View();
        }
    }
}
