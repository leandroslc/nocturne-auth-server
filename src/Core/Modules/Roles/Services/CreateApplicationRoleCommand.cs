namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public class CreateApplicationRoleCommand : ManageApplicationRoleCommand
    {
        public CreateApplicationRoleCommand()
        {
        }

        public CreateApplicationRoleCommand(string applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
