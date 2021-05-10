using Nocturne.Auth.Core.Services.Identity;

namespace Nocturne.Auth.Core.Modules.Roles
{
    public class UserRole
    {
        public UserRole(
            ApplicationUser user,
            Role role)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(role, nameof(role));

            UserId = user.Id;
            RoleId = role.Id;
        }

        private UserRole()
        {
        }

        public Role Role { get; private set; }

        public long UserId { get; private set; }

        public long RoleId { get; private set; }
    }
}
