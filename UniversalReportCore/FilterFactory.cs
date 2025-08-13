using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniversalReportCore;

namespace UniversalReportCore
{
    /// <summary>
    /// Factory class for building filter expressions based on selected filter keys.
    /// </summary>
    /// <typeparam name="T">The type of entity being filtered.</typeparam>
    public class FilterFactory<T> : IFilterFactory<T>
    {
        /// <summary>
        /// The filter provider supplying facet keys and their corresponding filter expressions.
        /// </summary>
        private readonly IFilterProvider<T> _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterFactory{T}"/> class.
        /// </summary>
        /// <param name="provider">The filter provider used to retrieve available filters.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="provider"/> is null.</exception>
        public FilterFactory(IFilterProvider<T> provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Builds a predicate expression that ORs filters within the same facet group and ANDs across different facet groups.
        /// </summary>
        /// <param name="selectedKeys">A collection of selected filter keys.</param>
        /// <returns>A combined predicate expression representing the applied filters.</returns>
        public Expression<Func<T, bool>> BuildPredicate(IEnumerable<string> selectedKeys)
        {
            var finalPredicate = PredicateBuilder.New<T>(true);

            // Iterate through each facet group (category of filters)
            foreach (var facetGroup in _provider.GetFacetKeys())
            {
                var orPredicate = PredicateBuilder.New<T>(false);

                // OR together filters within the same facet group
                foreach (var key in facetGroup.Value)
                {
                    if (selectedKeys.Contains(key) && _provider.Filters.TryGetValue(key, out var filterExpression))
                    {
                        orPredicate = orPredicate.Or(filterExpression);
                    }
                }

                // If no valid filters were selected in this group, skip it
                if (!orPredicate.IsStarted)
                    continue;

                // AND this facet group with the final predicate
                finalPredicate = finalPredicate.And(orPredicate);
            }

            return finalPredicate;
        }
    }
}
