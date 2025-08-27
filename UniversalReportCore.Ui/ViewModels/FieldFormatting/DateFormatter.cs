using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public class DateFormatter : IFieldFormatter
    {
        private readonly string _format;

        public DateFormatter(string? format = null)
        {
            _format = format ?? "yyyy-MM-dd";
        }

        public bool CanHandle(object value, Type propertyType)
        {
            if (value != null)
                return value is DateTime;

            return propertyType == typeof(DateTime) || propertyType == typeof(DateTime?);
        }

        public string Format(object value, Type propertyType)
        {
            if (value is DateTime dateTime)
                return dateTime.ToString(_format);

            return null; // or throw exception based on requirements
        }
    }
}
