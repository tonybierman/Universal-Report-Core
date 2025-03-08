using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniversalReportCore;

public class FilterFactory<T>
{
    private readonly IFilterProvider<T> _provider;

    public FilterFactory(IFilterProvider<T> provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary>
    /// Builds a predicate that ORs filters within groups and ANDs across groups.
    /// </summary>
    /// <param name="selectedKeys">A list of selected filter keys.</param>
    /// <returns>A combined predicate for filtering.</returns>
    public Expression<Func<T, bool>> BuildPredicate(IEnumerable<string> selectedKeys)
    {
        var finalPredicate = PredicateBuilder.New<T>(true);

        foreach (var facetGroup in _provider.GetFacetKeys())
        {
            var orPredicate = PredicateBuilder.New<T>(false);

            foreach (var key in facetGroup)
            {
                if (selectedKeys.Contains(key) && _provider.Filters.ContainsKey(key))
                {
                    orPredicate = orPredicate.Or(_provider.GetFilter(key));
                }
            }

            if (!orPredicate.IsStarted)
                continue; // Skip if no filters were selected in this group

            finalPredicate = finalPredicate.And(orPredicate);
        }

        return finalPredicate;
    }
}
