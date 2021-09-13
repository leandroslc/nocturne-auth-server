// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Generic;

namespace Nocturne.Auth.Core.Shared.Collections
{
    public interface IPagedCollection<TValue> : IPagedCollection, IEnumerable<TValue>
    {
    }
}
