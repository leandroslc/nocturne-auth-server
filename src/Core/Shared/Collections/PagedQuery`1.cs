namespace Nocturne.Auth.Core.Shared.Collections
{
    public class PagedCommand<TResult>
    {
        private int page = 1;

        public int Page
        {
            get => page;
            set => page = value > 0 ? value : 1;
        }

        public int PageSize { get; private set; } = 10;

        public PagedCommand<TResult> WithSize(int pageSize)
        {
            PageSize = pageSize;

            return this;
        }
    }
}
