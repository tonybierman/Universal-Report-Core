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

        foreach (var facetGroup in _provider.GetFacetKeys()) // Dictionary<string, List<string>>
        {
            var orPredicate = PredicateBuilder.New<T>(false);

            foreach (var key in facetGroup.Value) // Loop through keys in this category
            {
                if (selectedKeys.Contains(key) && _provider.Filters.TryGetValue(key, out var filterExpression))
                {
                    orPredicate = orPredicate.Or(filterExpression);
                }
            }

            if (!orPredicate.IsStarted)
                continue; // Skip this facet group if no filters were selected

            finalPredicate = finalPredicate.And(orPredicate);
        }

        return finalPredicate;
    }

}
