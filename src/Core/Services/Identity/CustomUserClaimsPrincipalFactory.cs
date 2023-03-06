// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Core.Services.Identity
{
    public class CustomUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            var userId = user.Id.ToString(CultureInfo.InvariantCulture);

            principal.SetClaim(OpenIddictConstants.Claims.Name, user.Name);
            principal.SetClaim(OpenIddictConstants.Claims.Subject, userId);
            principal.SetClaim(OpenIddictConstants.Claims.Username, user.UserName);

            return principal;
        }
    }
}
