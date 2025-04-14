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

        /// <summary>
        /// Determines the CSS alignment class for a column based on the item's properties.
        /// </summary>
        /// <param name="item">The object containing column data.</param>
        /// <param name="column">The column metadata with CssClass, PropertyName, and ViewModelName.</param>
        /// <returns>A Bootstrap CSS class (e.g., "text-start", "text-end").</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> or <paramref name="column"/> is null.</exception>
        private string GetCssAlignment(object item, IReportColumnDefinition column)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            // Try to get explicit CSS class from item, but only if CssClass is non-null/empty
            object cssAlign = null;
            if (!string.IsNullOrEmpty(column.CssClass))
            {
                cssAlign = item.GetType().GetProperty(column.CssClass)?.GetValue(item);
            }

            // If CssClass is null, empty, or not a string, determine alignment based on data type
            if (cssAlign as string is not { } align || string.IsNullOrWhiteSpace(align))
            {
                cssAlign = "text-start"; // Default to left alignment

                // Get field value from PropertyName or fall back to ViewModelName, with null checks
                object fieldVal = null;
                if (!string.IsNullOrEmpty(column.PropertyName))
                {
                    fieldVal = item.GetType().GetProperty(column.PropertyName)?.GetValue(item);
                }
                if (fieldVal == null && !string.IsNullOrEmpty(column.ViewModelName))
                {
                    fieldVal = item.GetType().GetProperty(column.ViewModelName)?.GetValue(item);
                }

                // Right-align null or numeric values
                if (fieldVal == null || Constants.NumericTypes.Contains(fieldVal.GetType()))
                {
                    cssAlign = "text-end";
                }
            }

            return cssAlign as string ?? cssAlign.ToString(); // Fallback to ToString if not string
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            string cssAlign = GetCssAlignment(Item, Column);

            if (_htmlHelper is HtmlHelper<dynamic> concreteHelper)
            {
                concreteHelper.Contextualize(ViewContext);  // Now it will work!
            }

            output.TagName = "td";
            var cssClasses = $"{cssAlign}" +
                            (Column.HideInPortrait ? " hide-in-portrait" : "");

            output.Attributes.Add("class", cssClasses);

            // Determine the correct ViewModel type or default to EntityFieldViewModel
            Type viewModelType = Column.ViewModelType ?? typeof(EntityFieldViewModel);

            // Use Activator.CreateInstance to dynamically instantiate the ViewModel
            object viewModelInstance;
            try
            {
                viewModelInstance = Activator.CreateInstance(viewModelType, Item, Column, Slug)!;
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
                        new FieldValueDisplayViewModel(Item, Column)) // TODO: Pass the column instead of the propertyname
            };

            output.Content.SetHtmlContent(content);
        }
    }
}
