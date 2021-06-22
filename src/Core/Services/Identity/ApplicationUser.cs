using Microsoft.AspNetCore.Identity;

namespace Nocturne.Auth.Core.Services.Identity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
            : base()
        {
            Enabled = true;
        }

        [ProtectedPersonalData]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string FirstName => Name?.Split(' ')?[0];
    }
}
