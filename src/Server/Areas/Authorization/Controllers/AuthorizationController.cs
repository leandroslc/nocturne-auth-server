using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nocturne.Auth.Core.Services.Identity;
using Nocturne.Auth.Core.Shared.Extensions;
using Nocturne.Auth.Server.Areas.Authorization.Models;
using Nocturne.Auth.Server.Configuration;
using Nocturne.Auth.Server.Services;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Nocturne.Auth.Server.Areas.Authorization.Controllers
{
    [Area("Authorization")]
    public class AuthorizationController : Controller
    {
        private readonly IOpenIddictApplicationManager applicationManager;
        private readonly IOpenIddictAuthorizationManager authorizationManager;
        private readonly IOpenIddictScopeManager scopeManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserClaimsService userClaimsService;

        public AuthorizationController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserClaimsService userClaimsService)
        {
            this.applicationManager = applicationManager;
            this.authorizationManager = authorizationManager;
            this.scopeManager = scopeManager;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userClaimsService = userClaimsService;
        }

        [HttpGet(AuthorizationEndpoints.Authorize)]
        [HttpPost(AuthorizationEndpoints.Authorize)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = GetOpenIdRequest();

            // Tries to retrieve the current user principal
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (HasUserAuthenticationFailed(result))
            {
                return UserAuthorizationDenied(request);
            }

            // If prompt=login was specified by the client application,
            // immediately return the user agent to the login page.
            if (request.HasPrompt(Prompts.Login))
            {
                var requestUriBuilder = new CurrentRequestUriBuilder(Request);

                // To avoid endless login -> authorization redirects, the prompt=login flag
                // is removed from the authorization request payload before redirecting the user.
                requestUriBuilder.ReplaceParameter(
                    Parameters.Prompt,
                    request.GetPrompts().Remove(Prompts.Login));

                return Challenge(requestUriBuilder.Build());
            }

            // If a max_age parameter was provided, ensure that the cookie is not too old.
            if (request.MaxAge is not null && IsAuthenticationTooOld(request, result))
            {
                return UserAuthorizationDenied(request);
            }

            return await ConsentAsync(request, result);
        }

        [HttpPost(AuthorizationEndpoints.Authorize)]
        [FormValueRequired("submit.Accept")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept()
        {
            var request = GetOpenIdRequest();
            var user = await GetUserAsync(User);
            var application = await GetApplicationAsync(request);
            var authorizations = await GetAuthorizationsAsync(request, application, user);

            var isExternalConsent = await applicationManager.HasConsentTypeAsync(application, ConsentTypes.External);
            var hasNoAuthorizations = !authorizations.Any();

            // Ensure a malicious user can't abuse this POST-only endpoint and force
            // it to return a valid response without the external authorization.
            if (hasNoAuthorizations && isExternalConsent)
            {
                return Forbid(
                    Errors.ConsentRequired,
                    "The logged in user is not allowed to access this client application.");
            }

            return await CreateAndSignInPrincipalAsync(
                request, user, application, authorizations);
        }

        [HttpPost(AuthorizationEndpoints.Authorize)]
        [FormValueRequired("submit.Deny")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Deny()
        {
            // The authorization grant has been denied by the resource owner.
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet(AuthorizationEndpoints.Logout)]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost(AuthorizationEndpoints.Logout)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            await signInManager.SignOutAsync();

            // Redirects the user agent to the post_logout_redirect_uri specified by
            // the client or to the RedirectUri if none was set.
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }

        [HttpPost(AuthorizationEndpoints.Token)]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = GetOpenIdRequest();

            if (request.IsPasswordGrantType())
            {
                return await ExchangeForPasswordGrant(request);
            }
            else if (request.IsClientCredentialsGrantType())
            {
                return await ExchangeForClientCredentialsGrant(request);
            }
            else if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                return await ExchangeForAuthorizationCodeOrRefreshTokenGrant();
            }

            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        private IActionResult UserAuthorizationDenied(OpenIddictRequest request)
        {
            // If the client application requested promptless authentication,
            // return an error indicating that the user is not logged in.
            if (request.HasPrompt(Prompts.None))
            {
                return Forbid(Errors.LoginRequired, "The user is not logged in.");
            }

            return Challenge(new CurrentRequestUriBuilder(Request).Build());
        }

        private async Task<IActionResult> ConsentAsync(
            OpenIddictRequest request,
            AuthenticateResult result)
        {
            var user = await GetUserAsync(result.Principal);
            var application = await GetApplicationAsync(request);
            var authorizations = await GetAuthorizationsAsync(request, application, user);
            var consentType = await applicationManager.GetConsentTypeAsync(application);

            var hasAuthorizations = authorizations.Any();

            switch (consentType)
            {
                // If the consent is external (e.g when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found.
                case ConsentTypes.External when !hasAuthorizations:
                    return Forbid(
                        Errors.ConsentRequired,
                        "The logged in user is not allowed to access this client application.");

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case ConsentTypes.Implicit:
                case ConsentTypes.External when hasAuthorizations:
                case ConsentTypes.Explicit when hasAuthorizations && !request.HasPrompt(Prompts.Consent):
                    return await CreateAndSignInPrincipalAsync(
                        request, user, application, authorizations);

                // At this point, no authorization was found and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(Errors.ConsentRequired, "Interactive user consent is required.");

                default:
                    return View(new AuthorizeViewModel
                    {
                        ApplicationName = await applicationManager.GetLocalizedDisplayNameAsync(application),
                        Scope = request.Scope
                    });
            }
        }

        private async Task<IActionResult> ExchangeForClientCredentialsGrant(OpenIddictRequest request)
        {
            var application = await GetApplicationAsync(request);

            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);

            identity.AddClaim(
                Claims.Subject,
                request.ClientId,
                Destinations.AccessToken,
                Destinations.IdentityToken);

            identity.AddClaim(
                Claims.Name,
                await applicationManager.GetDisplayNameAsync(application),
                Destinations.AccessToken,
                Destinations.IdentityToken);

            var principal = new ClaimsPrincipal(identity);
            principal.SetResources(await scopeManager.ListResourcesAsync(request.GetScopes()).ToListAsync());

            return SignInPrincipal(principal);
        }

        private async Task<IActionResult> ExchangeForPasswordGrant(OpenIddictRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return Forbid(
                    Errors.InvalidGrant,
                    "The username/password is invalid.");
            }

            // Validate the username/password and ensure the account is not locked out.
            var result = await signInManager.CheckPasswordSignInAsync(
                user, request.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Forbid(
                    Errors.InvalidGrant,
                    "The username/password is invalid.");
            }

            var principal = await CreateUserPrincipalAsync(user, request);

            return SignInPrincipal(principal);
        }

        private async Task<IActionResult> ExchangeForAuthorizationCodeOrRefreshTokenGrant()
        {
            var authenticationResult = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            if (!authenticationResult.Succeeded)
            {
                return Forbid(
                    Errors.InvalidGrant,
                    "Authentication failed");
            }

            var principal = authenticationResult.Principal;

            // Use the line below instead if the user should be invalidated if password/roles changes:
            // var user = signInManager.ValidateSecurityStampAsync(principal);
            var user = await userManager.GetUserAsync(principal);
            if (user is null)
            {
                return Forbid(
                    Errors.InvalidGrant,
                    "The token is no longer valid.");
            }

            // Ensure the user is still allowed to sign in.
            var cannotSignIn = !await signInManager.CanSignInAsync(user);
            if (cannotSignIn)
            {
                return Forbid(
                    Errors.InvalidGrant,
                    "The user is no longer allowed to sign in.");
            }

            await userClaimsService.AddClaimsDestinationsAsync(principal);

            return SignInPrincipal(principal);
        }

        private async Task<ClaimsPrincipal> CreateUserPrincipalAsync(
            ApplicationUser user,
            OpenIddictRequest request)
        {
            var principal = await signInManager.CreateUserPrincipalAsync(user);

            await userClaimsService.AddClaimsToPrincipalAsync(principal, request);
            await userClaimsService.AddClaimsDestinationsAsync(principal);

            return principal;
        }

        private async Task<string> GetOrCreatePermanetAuthorizationAsync<TApplication>(
            ClaimsPrincipal principal,
            ApplicationUser user,
            TApplication application,
            IEnumerable<object> authorizations)
        {
            var authorization = authorizations.LastOrDefault();

            if (authorization is null)
            {
                authorization = await authorizationManager.CreateAsync(
                    principal: principal,
                    subject: await userManager.GetUserIdAsync(user),
                    client: await applicationManager.GetIdAsync(application),
                    type: AuthorizationTypes.Permanent,
                    scopes: principal.GetScopes());
            }

            return await authorizationManager.GetIdAsync(authorization);
        }

        private OpenIddictRequest GetOpenIdRequest()
        {
            return HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
        }

        private Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return userManager.GetUserAsync(principal)
                ?? throw new InvalidOperationException("The user details cannot be retrieved.");
        }

        private async ValueTask<object> GetApplicationAsync(OpenIddictRequest request)
        {
            return await applicationManager.FindByClientIdAsync(request.ClientId)
                ?? throw new InvalidOperationException("Details concerning the calling client application cannot be found.");
        }

        private async Task<IReadOnlyCollection<object>> GetAuthorizationsAsync(
            OpenIddictRequest request,
            object application,
            ApplicationUser user)
        {
            return await authorizationManager.FindAsync(
                subject: await userManager.GetUserIdAsync(user),
                client: await applicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes())
                .ToListAsync();
        }

        private async Task<IActionResult> CreateAndSignInPrincipalAsync(
            OpenIddictRequest request,
            ApplicationUser user,
            object application,
            IEnumerable<object> authorizations)
        {
            var principal = await CreateUserPrincipalAsync(user, request);

            var authorizationId = await GetOrCreatePermanetAuthorizationAsync(
                principal, user, application, authorizations);

            principal.SetAuthorizationId(authorizationId);

            return SignInPrincipal(principal);
        }

        private ForbidResult Forbid(string error, string errorDescription)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = error,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription,
                }));
        }

        private ChallengeResult Challenge(string redirectUri)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = redirectUri,
                });
        }

        private IActionResult SignInPrincipal(ClaimsPrincipal principal)
        {
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private static bool HasUserAuthenticationFailed(AuthenticateResult result)
        {
            return result is null || !result.Succeeded;
        }

        private static bool IsAuthenticationTooOld(
            OpenIddictRequest request,
            AuthenticateResult result)
        {
            if (result.Properties?.IssuedUtc is null)
            {
                return false;
            }

            var issuedAge = DateTimeOffset.UtcNow - result.Properties.IssuedUtc;

            return issuedAge > TimeSpan.FromSeconds(request.MaxAge.Value);
        }
    }
}
