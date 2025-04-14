using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())[0]
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? enumValue.ToString();
        }
    }
}
