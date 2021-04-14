namespace Nocturne.Auth.Core.Web.TagHelpers
{
    public sealed class PagesTagHelperOptions
    {
        public string ContainerClass { get; set; } = "d-flex align-items-center justify-content-end mb-3";

        public string ContainerLabel { get; set; } = "Page list";

        public string NextPageIconClass { get; set; } = "bi-chevron-right";

        public string NextPageText { get; set; } = "Next page";

        public string PageListClass { get; set; } = "pagination mb-0";

        public string PageItemClass { get; set; } = "page-item";

        public string PageItemDisabledClass { get; set; } = "disabled";

        public string PageLinkClass { get; set; } = "page-link";

        public string PreviousPageIconClass { get; set; } = "bi-chevron-left";

        public string PreviousPageText { get; set; } = "Previous page";

        public string SummaryClass { get; set; } = "mr-2";

        public string SummaryItemCountClass { get; set; } = "mr-2";

        public string SummaryItemsCountText { get; set; } = "{0:n0} items";

        public string SummaryRangeText { get; set; } = "Showing {0:n0} &ndash; {1:n0}";
    }
}
