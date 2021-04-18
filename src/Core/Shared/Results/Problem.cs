namespace Nocturne.Auth.Core.Shared.Results
{
    public sealed class Problem
    {
        public Problem(string name, string description)
        {
            Check.NotNull(description, nameof(description));

            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }
    }
}
