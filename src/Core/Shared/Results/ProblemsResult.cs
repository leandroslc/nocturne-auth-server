using System;
using System.Collections.Generic;

namespace Nocturne.Auth.Core.Shared.Results
{
    public sealed class ProblemsResult : Result
    {
        public ProblemsResult(string description)
            : this(new[] { new Problem(null, description) })
        {
        }

        public ProblemsResult(IReadOnlyCollection<Problem> problems)
            : base(ok: false)
        {
            Problems = problems ?? Array.Empty<Problem>();
        }

        public IReadOnlyCollection<Problem> Problems { get; }
    }
}
