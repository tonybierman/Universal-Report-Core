using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public class DefaultFormatter : IFieldFormatter
    {
        public bool CanHandle(object value, Type propertyType) => true;

        public string Format(object value, Type propertyType)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
