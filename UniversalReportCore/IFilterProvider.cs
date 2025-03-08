using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public interface IFilterProvider<T>
    {
        string Key { get; }
        string DisplayName { get; }
        IEnumerable<Expression<Func<T, bool>>> GetAndFilters();
        IEnumerable<Expression<Func<T, bool>>> GetOrFilters();
    }

}
