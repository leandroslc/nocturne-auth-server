using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.OpenIddict
{
    public class Authorization : OpenIddictEntityFrameworkCoreAuthorization<string, Application, Token>
    {
    }
}
