using System;

namespace Nocturne.Auth.Authorization.Services
{
    public class UserAccessControlCommand
    {
        public string UserIdentifier { get; set; }

        internal void Verify()
        {
            if (string.IsNullOrWhiteSpace(UserIdentifier))
            {
                throw new InvalidOperationException("The user identifier can not be empty");
            }
        }
    }
}
