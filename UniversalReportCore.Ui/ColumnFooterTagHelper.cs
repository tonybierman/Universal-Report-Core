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
            // Set up the <th> element
            output.TagName = "th";

            // Apply CSS class based on visibility rules
            output.Attributes.SetAttribute("class", Column.HideInPortrait ? "text-end hide-in-portrait" : "text-end");

            // If the column is a display key, render an empty <th>
            if (Column.IsDisplayKey)
            {
                output.Content.SetContent(""); // Ensure an empty <th> is rendered
                return;
            }

            // Retrieve the aggregate value
            var value = Items?.Aggregates?.ContainsKey(Column.PropertyName) == true
                ? Items.Aggregates[Column.PropertyName]
                : null;

            // Render the formatted value if it exists
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
