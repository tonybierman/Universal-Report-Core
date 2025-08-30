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
        [HtmlAttributeName("column")]
        public IReportColumnDefinition Column { get; set; } = null!;

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

            if (Column == null || Column.PropertyName == null)
            {
                output.SuppressOutput();
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<th>");
            sb.Append("<div class=\"dropdown\">");
            sb.Append($"<button class=\"btn btn-secondary dropdown-toggle\" type=\"button\" id=\"dropdownMenuButton{Column.PropertyName}\" data-bs-toggle=\"dropdown\" aria-expanded=\"false\">");
            sb.Append("<i class=\"ms-Icon ms-Icon--Filter\" aria-hidden=\"true\"></i>");
            sb.Append("</button>");

            sb.Append($"<ul class=\"dropdown-menu small\" aria-labelledby=\"dropdownMenuButton{Column.PropertyName}\">");

            foreach (var filter in FilterOptions.OrderBy(t => t.Text))
            {
                var isChecked = filter.Selected ? "checked" : "";

                sb.Append($@"
                    <li class='dropdown-item'>
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
