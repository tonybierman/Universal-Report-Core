﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui.ViewModels.FieldFormatting
{
    public interface IFieldFormatter
    {
        string Format(object value, Type propertyType);
        bool CanHandle(object value, Type propertyType);
    }
}
