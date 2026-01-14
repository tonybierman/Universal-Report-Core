using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public class StringFormatter : IFieldFormatter
    {
        private readonly string _format;

        public StringFormatter(string? format = null)
        {
            _format = format ?? "{0}";
        }

        public bool CanHandle(object value, Type propertyType)
        {
            if (value != null)
                return value is string;

            return propertyType == typeof(string);
        }

        public string Format(object value, Type propertyType)
        {
            if (value is string str)
                return string.Format(_format, str);

            return string.Empty; // or throw exception based on requirements
        }
    }
}
