using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UniversalReportCore
{
    /// <summary>
    /// Provides filtering logic for a specific entity type.
    /// Supports faceted filtering with AND/OR combinations.
    /// </summary>
    /// <typeparam name="T">The type of entity being filtered.</typeparam>
    public interface IFilterProvider<T>
    {
        Dictionary<string, Expression<Func<T, bool>>> Filters { get; }
        Dictionary<string, List<string>> GetFacetKeys();
        Expression<Func<T, bool>> GetFilter(string key);
    }
}
