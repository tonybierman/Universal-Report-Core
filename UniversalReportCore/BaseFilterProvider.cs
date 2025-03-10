using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniversalReportCore
{
    public abstract class BaseFilterProvider<T> : IFilterProvider<T>
    {
        private readonly List<Facet<T>> _facets;

        protected BaseFilterProvider(List<Facet<T>> facets)
        {
            _facets = facets ?? throw new ArgumentNullException(nameof(facets));
        }

        public Dictionary<string, Expression<Func<T, bool>>> Filters => _facets
            .SelectMany(f => f.Values)
            .ToDictionary(v => v.Key, v => v.Filter);

        public Dictionary<string, List<string>> GetFacetKeys()
        {
            return _facets.ToDictionary(f => f.Name, f => f.Values.Select(v => v.Key).ToList());
        }

        public Expression<Func<T, bool>> GetFilter(string key)
        {
            return Filters[key];
        }
    }
}
