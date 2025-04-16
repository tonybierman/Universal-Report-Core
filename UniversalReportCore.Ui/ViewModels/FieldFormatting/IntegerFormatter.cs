using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public class IntegerFormatter : IFieldFormatter
    {
        public bool CanHandle(object value, Type propertyType)
        {
            if (value != null)
                return value is int || value is long;
            return propertyType == typeof(int) || propertyType == typeof(int?) ||
                   propertyType == typeof(long) || propertyType == typeof(long?);
        }

        public string Format(object value, Type propertyType)
        {
            return value != null ? value.ToString() : "0";
        }
    }
}
