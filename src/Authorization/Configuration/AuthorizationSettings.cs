using System;
using Nocturne.Auth.Authorization.Helpers;

namespace Nocturne.Auth.Authorization.Configuration
{
    public sealed class AuthorizationSettings
    {
        public AuthorizationSettings(
            AuthorizationOptions options)
        {
            SetPermissionsEndpoint(options);
        }

        public string AccessControlEndpoint { get; private set; }

        private void SetPermissionsEndpoint(AuthorizationOptions options)
        {
            AccessControlEndpoint = UrlHelper.Combine(
                options.Authority,
                options.AccessControlEndpoint ?? Constants.DefaultAccessControlEndpoint,
                options.ClientId);

            if (UrlHelper.IsValidUrl(AccessControlEndpoint) is false)
            {
                throw new InvalidOperationException(
                    "The formed URI for the access endpoint is invalid");
            }
        }
    }
}
