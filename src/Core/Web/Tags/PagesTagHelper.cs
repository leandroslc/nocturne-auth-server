// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Nocturne.Auth.Core.Shared.Collections;
using Nocturne.Auth.Core.Shared.Extensions;

namespace Nocturne.Auth.Core.Web.TagHelpers
{
    [HtmlTargetElement("pages", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PagesTagHelper : TagHelper
    {
        private readonly PagesTagHelperOptions options;
        private readonly IStringLocalizer<PagesTagHelper> localizer;

        public PagesTagHelper(
            IOptions<PagesTagHelperOptions> options,
            IStringLocalizer<PagesTagHelper> localizer)
        {
            this.options = options.Value;
            this.localizer = localizer;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public IPagedCollection Collection { get; set; }

        public string ParamName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("class", options.ContainerClass);
            output.Attributes.SetAttribute("aria-label", localizer[options.ContainerLabel]);

            output.Content.AppendHtml(CreateSummary());
            output.Content.AppendHtml(CreateList());

            base.Process(context, output);
        }

        private TagBuilder CreateList()
        {
            var list = new TagBuilder("ul");
            list.AddCssClass(options.PageListClass);

            var next = CreatePageItemLink(
                text: options.NextPageText,
                page: Collection.PageNumber + 1,
                disabled: Collection.IsLastPage,
                iconClass: options.NextPageIconClass);

            var previous = CreatePageItemLink(
                text: options.PreviousPageText,
                page: Collection.PageNumber - 1,
                disabled: Collection.IsFirstPage,
                iconClass: options.PreviousPageIconClass);

            list.InnerHtml
                .AppendHtml(previous)
                .AppendHtml(next);

            return list;
        }

        private TagBuilder CreatePageItemLink(
            string text,
            int page,
            bool disabled,
            string iconClass)
        {
            var item = new TagBuilder("li");
            item.AddCssClass(options.PageItemClass);

            var link = new TagBuilder("a");
            link.AddCssClass(options.PageLinkClass);
            link.Attributes.Add("title", localizer[text]);

            var icon = new TagBuilder("span");
            icon.AddCssClass(iconClass);
            icon.Attributes.Add("aria-hidden", "true");

            if (disabled)
            {
                item.AddCssClass(options.PageItemDisabledClass);
                link.Attributes.Add("tabindex", "-1");
            }

            link.Attributes.Add("href", disabled ? "#" : GetUrlForPage(page));
            link.InnerHtml.AppendHtml(icon);

            item.InnerHtml.AppendHtml(link);

            return item;
        }

        private TagBuilder CreateSummary()
        {
            var summary = new TagBuilder("div");
            summary.AddCssClass(options.SummaryClass);
            summary.Attributes.Add("style", "font-size: 0.9em");

            summary.InnerHtml.AppendHtml(CreateItemsCountSummary());

            if (Collection.Total > 1)
            {
                summary.InnerHtml.AppendHtml(CreateItemsRangeSummary());
            }

            return summary;
        }

        private TagBuilder CreateItemsCountSummary()
        {
            var items = new TagBuilder("span");

            items.AddCssClass(options.SummaryItemCountClass);

            items.InnerHtml.Append(
                localizer[options.SummaryItemsCountText, Collection.Total]);

            if (Collection.Total > 1)
            {
                items.InnerHtml.Append(".");
            }

            return items;
        }

        private TagBuilder CreateItemsRangeSummary()
        {
            var range = new TagBuilder("span");

            range.InnerHtml.AppendHtml(
                localizer[options.SummaryRangeText, Collection.FirstItem, Collection.LastItem]);

            return range;
        }

        private string GetUrlForPage(int page)
        {
            return ViewContext.HttpContext.Request
                .CreateUrlWithNewQuery((ParamName, page.ToString()));
        }
    }
}
