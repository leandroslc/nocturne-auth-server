using Microsoft.AspNetCore.Identity;
using Nocturne.Auth.Core.Shared.Models;

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

        [ProtectedPersonalData]
        public CPF CPF { get; set; }

        public bool Enabled { get; set; }

        public string FirstName => Name?.Split(' ')?[0];
    }
}
