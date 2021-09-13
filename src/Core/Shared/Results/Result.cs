// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Shared.Results
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
