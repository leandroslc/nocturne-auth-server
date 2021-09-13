// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

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
