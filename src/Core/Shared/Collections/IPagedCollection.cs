// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections;

namespace Nocturne.Auth.Core.Shared.Collections
{
    public interface IPagedCollection : ICollection
    {
        long Total { get; }

        int PageSize { get; }

        int PageNumber { get; }

        int PageCount { get; }

        long FirstItem { get; }

        long LastItem { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }
}
