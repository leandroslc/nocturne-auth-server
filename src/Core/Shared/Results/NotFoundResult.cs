namespace Nocturne.Auth.Core.Shared.Results
{
    public sealed class NotFoundResult : Result
    {
        public NotFoundResult(string description)
            : base(ok: false)
        {
            Description = description;
        }

        public string Description { get; }
    }
}
