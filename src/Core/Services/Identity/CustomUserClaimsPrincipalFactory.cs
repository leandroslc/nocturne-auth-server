using System.Security.Claims;
using System.Threading.Tasks;
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

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            principal.SetClaim(OpenIddictConstants.Claims.Name, user.Name);

            return principal;
        }
    }
}
