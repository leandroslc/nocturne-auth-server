using OpenIddict.EntityFrameworkCore.Models;

namespace Nocturne.Auth.Core.Services.OpenIddict
{
    public class Authorization : OpenIddictEntityFrameworkCoreAuthorization<string, Application, Token>
    {
    }
}
