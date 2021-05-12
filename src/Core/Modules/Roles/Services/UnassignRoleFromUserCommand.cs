namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignRoleFromUserCommand
    {
        public long? UserId { get; set; }

        public long? RoleId { get; set; }
    }
}
