using System;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCommand
    {
        public string AccessToken { get; set; }

        public string UserIdentifier { get; set; }

        internal void Verify()
        {
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                throw new InvalidOperationException("Access token can not be empty");
            }

            if (string.IsNullOrWhiteSpace(UserIdentifier))
            {
                throw new InvalidOperationException("The user identifier can not be empty");
            }
        }
    }
}
