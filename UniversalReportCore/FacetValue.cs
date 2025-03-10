using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public class FacetValue<T>
    {
        public string Key { get; }
        public Expression<Func<T, bool>> Filter { get; }

        public FacetValue(string key, Expression<Func<T, bool>> filter)
        {
            Key = key;
            Filter = filter;
        }
    }
}
