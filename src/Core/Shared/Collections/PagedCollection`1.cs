// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections;

namespace Nocturne.Auth.Core.Shared.Collections
{
    public class PagedCollection<TValue> : IPagedCollection<TValue>
    {
        private readonly ICollection<TValue> subset;

        public PagedCollection(
            ICollection<TValue> subset,
            int pageNumber,
            int pageSize,
            long total)
        {
            if (subset is null)
            {
                throw new ArgumentNullException(nameof(subset));
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(pageSize), "Cannot be less than 1");
            }

            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(total), "Cannot be less than zero");
            }

            this.subset = subset;

            Total = total;
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;

            PageCount = Total == 0 ? 0 : (int)Math.Ceiling((decimal)Total / PageSize);
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItem = ((PageNumber - 1) * PageSize) + 1;
            LastItem = IsLastPage ? Total : PageNumber * PageSize;
            HasNextPage = !IsLastPage;
            HasPreviousPage = !IsFirstPage;
        }

        public long Total { get; }

        public int PageSize { get; }

        public int PageNumber { get; }

        public int PageCount { get; }

        public long FirstItem { get; }

        public long LastItem { get; }

        public bool HasPreviousPage { get; }

        public bool HasNextPage { get; }

        public bool IsFirstPage { get; }

        public bool IsLastPage { get; }

        public int Count => PageCount;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        public void CopyTo(Array array, int index)
        {
            Check.NotNull(array, nameof(array));

            bool hasEnoughElementsAfterIndex = array.Length - index >= Count;

            if (index < 0 || hasEnoughElementsAfterIndex is false)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var currentSubsetIndex = 0;

            foreach (var item in subset)
            {
                array.SetValue(item, currentSubsetIndex + index);

                currentSubsetIndex += 1;
            }

            throw new NotImplementedException();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return subset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
