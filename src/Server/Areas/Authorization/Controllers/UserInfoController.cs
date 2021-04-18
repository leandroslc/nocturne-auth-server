using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Server.Configuration;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Server.Areas.Authorization.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserInfoController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet(AuthorizationEndpoints.UserInfo)]
        [HttpPost(AuthorizationEndpoints.UserInfo)]
        [IgnoreAntiforgeryToken]
        [Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var user = await userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge(
                    Errors.InvalidToken,
                    "The specified access token is bound to an account that no longer exists.");
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await userManager.GetUserIdAsync(user)
            };

            if (User.HasScope(Scopes.Email))
            {
                claims[Claims.Email] = await userManager.GetEmailAsync(user);
                claims[Claims.EmailVerified] = await userManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Phone))
            {
                claims[Claims.PhoneNumber] = await userManager.GetPhoneNumberAsync(user);
                claims[Claims.PhoneNumberVerified] = await userManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Roles))
            {
                claims[Claims.Role] = await userManager.GetRolesAsync(user);
            }

            // The complete list of standard claims supported by the OpenID Connect specification
            // can be found in http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
            return Ok(claims);
        }

        private ChallengeResult Challenge(string error, string errorDescription)
        {
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription,
                }));
        }
    }
}
