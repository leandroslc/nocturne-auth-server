namespace Nocturne.Auth.Core.Shared.Results
{
    public sealed class SuccessResult : Result
    {
        public SuccessResult()
            : base(ok: true)
        {
        }
    }
}
