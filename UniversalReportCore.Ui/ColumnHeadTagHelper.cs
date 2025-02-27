using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui
{
    [HtmlTargetElement("column-heading")]
    public class ColumnHeadTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ColumnHeadTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("column")]
        public IReportColumnDefinition Column { get; set; } = null!;

        [HtmlAttributeName("model")]
        public IReportQueryParams? Model { get; set; }

        [HtmlAttributeName("sort")]
        public string Sort { get; set; }

        [HtmlAttributeName("page")]
        public string Page { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            // Set up the <th> element
            output.TagName = "th";
            output.Attributes.SetAttribute("class", $"align-middle {(Column.HideInPortrait ? "hide-in-portrait" : "")}");

            // If the column is sortable, generate the <a> link
            if (Column.IsSortable)
            {
                var newSortOrder = Column.IsSortDescending ? $"{Column.PropertyName}Asc" : $"{Column.PropertyName}Desc";

                var url = urlHelper.Page(Page, new
                {
                    slug = Model?.Slug.Value,
                    ipp = Model?.Ipp.Value,
                    sortOrder = newSortOrder,
                    cohortIds = Model?.CohortIds.Value
                });

                var linkTag = new TagBuilder("a");
                linkTag.Attributes["href"] = url;
                linkTag.InnerHtml.Append(Column.DisplayName);

                output.Content.AppendHtml(linkTag);

                // Show sort indicator
                if (Column.PropertyName == Sort.Replace("Asc", "").Replace("Desc", ""))
                {
                    var sortIndicator = Column.IsSortDescending ? "↓" : "↑";
                    var spanTag = new TagBuilder("span");
                    spanTag.AddCssClass("sort-indicator");
                    spanTag.InnerHtml.Append(sortIndicator);
                    output.Content.AppendHtml(spanTag);
                }
            }
            else
            {
                // Just render the column display name if it's not sortable
                output.Content.Append(Column.DisplayName);
            }
        }
    }
}
