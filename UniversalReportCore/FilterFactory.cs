using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit; // For PredicateBuilder

namespace UniversalReportCore
{
    /// <summary>
    /// A factory class for constructing dynamic LINQ predicates used for filtering collections of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The entity type that the filter will be applied to.</typeparam>
    public class FilterFactory<T>
    {
        /// <summary>
        /// Builds a composite predicate using the provided filter provider.
        /// The resulting predicate applies:
        /// - **AND conditions** to ensure all conditions must be met.
        /// - **OR conditions** to allow for alternative criteria within the AND block.
        /// </summary>
        /// <param name="provider">The filter provider that supplies AND and OR conditions.</param>
        /// <returns>An `Expression<Func<T, bool>>` representing the combined filter criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provider is null.</exception>
        public Expression<Func<T, bool>> BuildPredicate(IFilterProvider<T> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "Filter provider cannot be null.");
            }

            // Start with a predicate that always evaluates to TRUE (for AND conditions)
            var predicate = PredicateBuilder.New<T>(true);

            // Apply all AND filters
            foreach (var andFilter in provider.GetAndFilters())
            {
                predicate = predicate.And(andFilter);
            }

            // Apply OR filters as a separate block
            var orFilters = provider.GetOrFilters();
            if (orFilters.Any())
            {
                // Start with a predicate that always evaluates to FALSE (for OR conditions)
                var orPredicate = PredicateBuilder.New<T>(false);

                // Combine OR filters
                foreach (var orFilter in orFilters)
                {
                    orPredicate = orPredicate.Or(orFilter);
                }

                // Combine OR block with the existing AND conditions
                predicate = predicate.And(orPredicate);
            }

            return predicate;
        }
    }
}
