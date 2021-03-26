using System;
using System.Collections;
using System.Collections.Generic;

namespace Nocturne.Auth.Core.Collections
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
            FirstItem =  ((PageNumber - 1) * PageSize) + 1;
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
