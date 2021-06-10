using System;
using Nocturne.Auth.Authorization.Helpers;

namespace Nocturne.Auth.Authorization.Configuration
{
    public sealed class AuthorizationSettings
    {
        public AuthorizationSettings(AuthorizationOptions options)
        {
            AccessControlEndpoint = GetAccessControlEndpoint(options);
            ClientId = GetClientId(options);
            GetAccessTokenAsync = GetAccessTokenMethod(options);
            CacheExpirationTime = options.CacheExpirationTime;
        }

        public string AccessControlEndpoint { get; }

        public string ClientId { get; }

        public GetAccessToken GetAccessTokenAsync { get; }

        public TimeSpan CacheExpirationTime { get; }

        private static string GetClientId(AuthorizationOptions options)
        {
            return string.IsNullOrEmpty(options.ClientId) is false
                ? options.ClientId
                : throw new InvalidOperationException("Client Id can not be null");
        }

        private static string GetAccessControlEndpoint(AuthorizationOptions options)
        {
            var endpoint = UrlHelper.Combine(
                options.Authority,
                options.AccessControlEndpoint ?? Constants.DefaultAccessControlEndpoint,
                options.ClientId);

            if (UrlHelper.IsValidUrl(endpoint) is false)
            {
                throw new InvalidOperationException(
                    "The formed URI for the access endpoint is invalid");
            }

            return endpoint;
        }

        private static GetAccessToken GetAccessTokenMethod(AuthorizationOptions options)
        {
            return options.GetAccessToken
                ?? throw new InvalidOperationException(
                    "A method should be specified for getting a valid access token");
        }
    }
}
