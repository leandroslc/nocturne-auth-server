// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

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
