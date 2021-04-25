namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public class CreatePermissionCommand : ManagePermissionCommand
    {
        public CreatePermissionCommand()
        {
        }

        public CreatePermissionCommand(string applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
