namespace Nocturne.Auth.Core.Modules.Roles
{
    public class RoleApplication
    {
        public RoleApplication(
            string id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public RoleApplication()
        {
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
