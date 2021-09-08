// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Admin.Configuration.Constants;

namespace Nocturne.Auth.Admin.Controllers
{
    [Route("")]
    [Authorize(Policy = Policies.GeneralAccess)]
    public class HomeController : Controller
    {
        [HttpGet("", Name = RouteNames.Home)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
