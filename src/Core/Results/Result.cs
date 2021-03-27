namespace Nocturne.Auth.Core.Results
{
    public class Result
    {
        public Result(bool ok)
        {
            Ok = ok;
        }

        public bool Ok { get; }

        public static SuccessResult Success => new();

        public static ProblemsResult Fail(string description) => new(description);

        public static NotFoundResult NotFound(string description = null) => new(description);
    }
}
