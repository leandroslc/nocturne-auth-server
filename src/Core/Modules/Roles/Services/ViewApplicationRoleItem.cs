namespace Nocturne.Auth.Core.Modules.Roles.Services
{
    public sealed class ViewApplicationRoleItem
    {
        public ViewApplicationRoleItem(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
