namespace Nocturne.Auth.Core.Results
{
    public sealed class Problem
    {
        public Problem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }
    }
}
