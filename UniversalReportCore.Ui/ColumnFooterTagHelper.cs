using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using System.Linq;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCoreUi
{
    [HtmlTargetElement("column-footer")]
    public class ColumnFooterTagHelper : TagHelper
    {
        [HtmlAttributeName("column")]
        public IReportColumnDefinition Column { get; set; } = null!;

        [HtmlAttributeName("model")]
        public IReportQueryParams? Model { get; set; }

        [HtmlAttributeName("items")]
        public IPaginatedList Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Skip rendering for display key columns
            if (Column.IsDisplayKey)
            {
                output.SuppressOutput();
                return;
            }

            var value = Items?.Aggregates?.ContainsKey(Column.PropertyName) == true
                ? Items.Aggregates[Column.PropertyName]
                : null;

            // Set up the <th> element
            output.TagName = "th";
            output.Attributes.SetAttribute("class", Column.HideInPortrait ? "hide-in-portrait" : "");

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
