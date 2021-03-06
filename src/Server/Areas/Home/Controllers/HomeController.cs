// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Server.Configuration.Constants;

namespace Nocturne.Auth.Server.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/", Name = RouteNames.Home)]
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }
    }
}
