using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Security.Policy;
using UniversalReportCore;
using UniversalReportCore.Ui.ViewModels;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui
{
    [HtmlTargetElement("render-column")]
    public class ColumnTagHelper : TagHelper
    {
        public IReportColumnDefinition Column { get; set; }
        public IEntityViewModel<int> Item { get; set; }
        public string Slug { get; set; }

        private readonly IHtmlHelper<dynamic> _htmlHelper;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public ColumnTagHelper(IHtmlHelper<dynamic> htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (_htmlHelper is HtmlHelper<dynamic> concreteHelper)
            {
                concreteHelper.Contextualize(ViewContext);  // Now it will work!
            }

            output.TagName = "td";
            var cssClasses = "align-middle " +
                            (Column.HideInPortrait ? " hide-in-portrait" : "");

            output.Attributes.Add("class", cssClasses);

            IHtmlContent content = Column switch
            {
                _ when !string.IsNullOrEmpty(Column.RenderPartial) => await _htmlHelper.PartialAsync(Column.RenderPartial, new EntityFieldViewModelBase<IEntityViewModel<int>>(Item) { Slug = Slug }),
                _ => await _htmlHelper.PartialAsync("_FieldValueDisplayPartial", new FieldValueDisplayViewModel(Item, Column.ViewModelName ?? Column.PropertyName))
            };

            output.Content.SetHtmlContent(content);
        }
    }
}
