using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public static class Constants
    {
        public static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(int), typeof(int?),
            typeof(long), typeof(long?),
            typeof(double), typeof(double?),
            typeof(float), typeof(float?),
            typeof(decimal), typeof(decimal?)
        };
    }
}
