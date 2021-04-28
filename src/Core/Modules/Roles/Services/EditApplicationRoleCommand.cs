namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class EditApplicationRoleCommand : ManageApplicationRoleCommand
    {
        public EditApplicationRoleCommand()
        {
        }

        public EditApplicationRoleCommand(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
            ApplicationId = role.ApplicationId;
        }

        public long? Id { get; set; }
    }
}
