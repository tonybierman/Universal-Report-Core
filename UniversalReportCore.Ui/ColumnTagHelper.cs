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
    [HtmlTargetElement("column-field")]
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

            // Determine the correct ViewModel type or default to EntityFieldViewModel
            Type viewModelType = Column.ViewModelType ?? typeof(EntityFieldViewModel);

            // Use Activator.CreateInstance to dynamically instantiate the ViewModel
            object viewModelInstance;
            try
            {
                viewModelInstance = Activator.CreateInstance(viewModelType, Item)!;
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException(
                    $"Constructor not found for type '{viewModelType.FullName}' with parameter type '{Item?.GetType().FullName ?? "null"}'. " +
                    $"Ensure the constructor exists and matches the expected parameter.",
                    ex
                );
            }

            IHtmlContent content = Column switch
            {
                _ when !string.IsNullOrEmpty(Column.RenderPartial) =>
                    await _htmlHelper.PartialAsync(Column.RenderPartial, 
                        viewModelInstance),

                _ => await _htmlHelper.PartialAsync("_FieldValueDisplayPartial",
                        new FieldValueDisplayViewModel(Item, Column.ViewModelName ?? Column.PropertyName))
            };

            output.Content.SetHtmlContent(content);
        }
    }
}
