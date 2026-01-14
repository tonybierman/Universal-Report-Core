using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCoreUi
{
    [HtmlTargetElement("cohort-footer")]
    public class CohortFooterTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public CohortFooterTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("column")]
        public IReportColumnDefinition Column { get; set; } = null!;

        [HtmlAttributeName("model")]
        public IReportQueryParams? Model { get; set; }

        [HtmlAttributeName("cohort-id")]
        public int CohortId { get; set; }

        [HtmlAttributeName("items")]
        public IPaginatedList? Items { get; set; }

        [HtmlAttributeName("cohorts")]
        public ICohort[]? Cohorts { get; set; }

        [HtmlAttributeName("page")]
        public string Page { get; set; } = "/Reports/Index";

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            // Set up the <td> element
            output.TagName = "td";
            output.Attributes.SetAttribute("class", Column.HideInPortrait ? "text-end hide-in-portrait" : "text-end");

            if (Column.IsDisplayKey)
            {
                var cohort = Cohorts?.FirstOrDefault(c => c.Id == CohortId);
                var url = urlHelper.Page(Page, new
                {
                    slug = Model?.Slug.Value,
                    ipp = Model?.Ipp.Value,
                    sortOrder = Model?.SortOrder.Value,
                    cohortIds = CohortId
                });

                var linkTag = new TagBuilder("a");
                linkTag.AddCssClass("m-0 btn-sm btn-link");
                linkTag.Attributes["href"] = url;
                linkTag.InnerHtml.Append(cohort != null ? cohort.Name : $"Cohort {CohortId} Totals");

                output.Content.AppendHtml(linkTag);
            }
            else
            {
                var cohortKey = $"{Column.PropertyName}_{CohortId}";
                var value = Items?.Aggregates?.ContainsKey(cohortKey) == true
                    ? Items.Aggregates[cohortKey]
                    : null;

                if (value != null)
                {
                    string formattedValue = value is double or float or decimal
                        ? $"{value:F2}" // Format with 2 decimal places
                        : $"{value}";   // Default formatting for other types

                    output.Content.SetContent(formattedValue);
                }
            }
        }
    }
}
