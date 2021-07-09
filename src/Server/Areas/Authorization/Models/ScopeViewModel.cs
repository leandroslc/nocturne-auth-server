namespace Nocturne.Auth.Server.Areas.Authorization.Models
{
    public class ScopeViewModel
    {
        public ScopeViewModel(
            string name,
            string description)
        {
            Name = name;
            Description = description;
            Required = true;
        }

        public string Name { get; }

        public string Description { get; }

        public bool Required { get; }
    }
}
