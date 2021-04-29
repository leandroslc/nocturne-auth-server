namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class EditApplicationRoleCommand : ManageApplicationRoleCommand
    {
        public EditApplicationRoleCommand()
        {
        }

        public EditApplicationRoleCommand(Role role, RoleApplication application)
            : base(application)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public long? Id { get; set; }
    }
}
