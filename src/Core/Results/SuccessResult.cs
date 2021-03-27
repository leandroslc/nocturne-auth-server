namespace Nocturne.Auth.Core.Results
{
    public sealed class SuccessResult : Result
    {
        public SuccessResult()
            : base(ok: true)
        {
        }
    }
}
