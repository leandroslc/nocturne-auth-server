// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;

namespace Nocturne.Auth.Server.Areas.Home.Controllers
{
    [Area("Home")]
    [Route("error")]
    public class ErrorsController : Controller
    {
        [HttpGet("unexpected")]
        public IActionResult InternalServerError()
        {
            return View();
        }
    }
}
