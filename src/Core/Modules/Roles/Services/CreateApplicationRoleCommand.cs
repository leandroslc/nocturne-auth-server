namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class CreateApplicationRoleCommand : ManageApplicationRoleCommand
    {
        public CreateApplicationRoleCommand()
        {
        }

        public CreateApplicationRoleCommand(RoleApplication application)
            : base(application)
        {
        }
    }
}
