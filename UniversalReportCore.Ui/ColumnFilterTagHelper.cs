using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text;

namespace UniversalReportCore.Ui
{
    [HtmlTargetElement("column-filter")]
    public class ColumnFilterTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ColumnFilterTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("options")]
        public List<SelectListItem> FilterOptions { get; set; } = new();

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (FilterOptions == null || FilterOptions.Count == 0)
            {
                output.SuppressOutput();
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<th>");
            sb.Append("<ul class=\"list-group list-group-flush small\">");

            foreach (var filter in FilterOptions.OrderBy(t => t.Text))
            {
                var isChecked = filter.Selected ? "checked" : "";

                sb.Append($@"
                    <li class='list-group-item bg-transparent text-secondary border-0 py-1 px-2'>
                        <label class='d-flex align-items-center'>
                            <input type='checkbox' 
                                   name='filters' 
                                   value='{filter.Value}' 
                                   {isChecked}
                                   class='me-2' 
                                   onchange='this.form.submit()' />
                            {filter.Text}
                        </label>
                    </li>");
            }

            sb.Append("</ul>");
            sb.Append("</th>");
            output.TagName = null; // Prevent wrapping element
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
