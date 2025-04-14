using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public dynamic Item { get; set; }

        /// <summary>
        /// Gets or sets the property name to be accessed.
        /// </summary>
        public string PropertyName { get; set; }

        public IReportColumnDefinition ColumnDefinition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValueDisplayViewModel"/> class.
        /// </summary>
        /// <param name="item">The IEntityViewModel<int> object.</param>
        /// <param name="propertyName">The property name to be accessed.</param>
        public FieldValueDisplayViewModel(IEntityViewModel<int> item, IReportColumnDefinition column)
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
            var property = type?.GetProperty(PropertyName);
            return property?.GetValue(Item);
        }
    }
}

