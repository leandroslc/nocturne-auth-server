using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Nocturne.Auth.Admin.Configuration.Constants
{
    public static class ApplicationConstants
    {
        public const string Identifier = "authorization-server-admin";

        public const string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        public const string AuthenticationChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }
}
