using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.OpenIddict
{
    public class Token : OpenIddictEntityFrameworkCoreToken<string, Application, Authorization>
    {
    }
}
