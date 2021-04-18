using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.Services.OpenIddict
{
    public class Application : OpenIddictEntityFrameworkCoreApplication<string, Authorization, Token>
    {
    }
}
