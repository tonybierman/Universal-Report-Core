using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.Ui.ViewModels.FieldFormatting;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui.ViewModels
{
    /// <summary>
    /// Represents a view model for dynamically accessing properties of an object.
    /// </summary>
    public class FieldValueDisplayViewModel
    {
        /// <summary>
        /// Gets or sets the dynamic object containing the property.
        /// </summary>
        public BaseEntityViewModel Item { get; set; }

        /// <summary>
        /// Gets or sets the property name to be accessed.
        /// </summary>
        public string PropertyName { get; set; }

        public IReportColumnDefinition ColumnDefinition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValueDisplayViewModel"/> class.
        /// </summary>
        /// <param name="item">The IEntityViewModel<int> object.</param>
        /// <param name="column">The column definition.</param>
        public FieldValueDisplayViewModel(BaseEntityViewModel item, IReportColumnDefinition column)
        {
            Item = item;
            ColumnDefinition = column;
            PropertyName = column.ViewModelName ?? column.PropertyName;
        }

        /// <summary>
        /// Retrieves the value of the specified property from the dynamic object.
        /// </summary>
        /// <returns>The property value if found; otherwise, null.</returns>
        public object? GetValue()
        {
            var type = Item?.GetType();
            var property = type != null && PropertyName != null ? type.GetProperty(PropertyName) : null;
            if (ColumnDefinition.ValueSelector != null && Item != null)
            {
                return ColumnDefinition.ValueSelector(Item, type, property);
            }

            return property?.GetValue(Item);
        }

        private static readonly IFieldFormatter[] Formatters = new IFieldFormatter[]
        {
            new IntegerFormatter(),
            new DecimalFormatter(),
            new DefaultFormatter()
        };

        /// <summary>
        /// Formats the field value based on its type or property type using a pipeline of formatters.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <returns>The formatted string representation of the value.</returns>
        public string FormatFieldValue(object value, Type propertyType)
        {
            foreach (var formatter in Formatters)
            {
                if (formatter.CanHandle(value, propertyType))
                {
                    return formatter.Format(value, propertyType);
                }
            }
            return string.Empty; // Fallback, should never reach here due to DefaultFormatter
        }
    }
}