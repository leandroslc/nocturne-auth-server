namespace Nocturne.Auth.Core.Collections
{
    public interface IPagedCollection
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
