namespace Nocturne.Auth.Authorization.Configuration
{
    public class AuthorizationOptions
    {
        public string ClientId { get; set; }

        public string Authority { get; set; }

        public string AccessControlEndpoint { get; set; } = Constants.DefaultAccessControlEndpoint;
    }
}
