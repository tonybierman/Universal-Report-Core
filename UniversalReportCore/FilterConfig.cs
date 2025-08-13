using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public record FilterConfig<T>(
        IFilterProvider<T> FilterProvider,
        FilterFactory<T> FilterFactory,
        string[] FilterKeys) : IFilterConfig<T>;
}
