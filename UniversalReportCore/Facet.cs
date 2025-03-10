using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public class Facet<T>
    {
        public string Name { get; }
        public List<FacetValue<T>> Values { get; }

        public Facet(string name, List<FacetValue<T>> values)
        {
            Name = name;
            Values = values;
        }
    }
}
