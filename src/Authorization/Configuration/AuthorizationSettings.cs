using System;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Authorization.Helpers;

namespace Nocturne.Auth.Authorization.Configuration
{
    public sealed class AuthorizationSettings
    {
        public AuthorizationSettings(
            IOptionsMonitor<AuthorizationOptions> options)
        {
            var currentOptions = options.CurrentValue;

            AccessControlEndpoint = GetAccessControlEndpoint(currentOptions);
            ClientId = GetClientId(currentOptions);
        }

        public string AccessControlEndpoint { get; }

        public string ClientId { get; }

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
    }
}
