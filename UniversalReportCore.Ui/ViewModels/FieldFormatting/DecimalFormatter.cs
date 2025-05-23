﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public class DecimalFormatter : IFieldFormatter
    {
        public bool CanHandle(object value, Type propertyType)
        {
            if (value != null)
                return value is double || value is float || value is decimal;
            return propertyType == typeof(double) || propertyType == typeof(double?) ||
                   propertyType == typeof(decimal) || propertyType == typeof(decimal?) ||
                   propertyType == typeof(float) || propertyType == typeof(float?);
        }

        public string Format(object value, Type propertyType)
        {
            return value != null ? string.Format("{0:N2}", value) : string.Format("{0:N2}", 0);
        }
    }
}
