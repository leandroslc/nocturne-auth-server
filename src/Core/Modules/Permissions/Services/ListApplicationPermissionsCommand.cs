namespace Nocturne.Auth.Core.Modules.Permissions.Services
{
    public sealed class ListApplicationPermissionsCommand
    {
        public string ApplicationId { get; set; }

        public string Name { get; set; }
    }
}
