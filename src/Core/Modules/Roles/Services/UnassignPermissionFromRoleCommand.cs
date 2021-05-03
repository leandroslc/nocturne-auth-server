namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignPermissionFromRoleCommand
    {
        public long? RoleId { get; private set; }

        public long? PermissionId { get; private set; }
    }
}
