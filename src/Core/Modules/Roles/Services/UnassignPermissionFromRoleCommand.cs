namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class UnassignPermissionFromRoleCommand
    {
        public long? RoleId { get; set; }

        public long? PermissionId { get; set; }
    }
}
