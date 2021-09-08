// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

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
