using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Nocturne.Auth.Authorization.Configuration
{
    public delegate Task<string> GetAccessToken(HttpContext context);

    public class AuthorizationOptions
    {
        public string ClientId { get; set; }

        public string Authority { get; set; }

        public string AccessControlEndpoint { get; set; } = Constants.DefaultAccessControlEndpoint;

        public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(30);

        public GetAccessToken GetAccessToken { get; set; }
    }
}
