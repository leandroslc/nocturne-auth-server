namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewApplicationRoleItem
    {
        public ViewApplicationRoleItem(Role role)
        {
            Name = role.Name;
            Description = role.Description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
