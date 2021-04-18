using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.OpenIddict
{
    public class Application : OpenIddictEntityFrameworkCoreApplication<string, Authorization, Token>
    {
    }
}
