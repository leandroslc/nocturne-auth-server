using System.Security.Claims;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace Nocturne.Auth.Server.Services
{
    public interface IUserClaimsService
    {
        Task AddClaimsDestinationsAsync(ClaimsPrincipal principal);

        Task AddClaimsToPrincipalAsync(ClaimsPrincipal principal, OpenIddictRequest request);
    }
}
