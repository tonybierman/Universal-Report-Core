using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniversalReportCore
{
    /// <summary>
    /// Base class for filter providers, managing facets and filters for a specific entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity being filtered.</typeparam>
    public abstract class BaseFilterProvider<T> : IFilterProvider<T>
    {
        /// <summary>
        /// A collection of facets containing filterable values for the entity type.
        /// </summary>
        private readonly List<Facet<T>> _facets;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFilterProvider{T}"/> class.
        /// </summary>
        /// <param name="facets">A list of facets defining available filters.</param>
        /// <exception cref="ArgumentNullException">Thrown if facets list is null.</exception>
        protected BaseFilterProvider(List<Facet<T>> facets)
        {
            _facets = facets ?? throw new ArgumentNullException(nameof(facets));
        }

        /// <summary>
        /// Retrieves a dictionary of all available filters, mapping keys to their corresponding expressions.
        /// </summary>
        public Dictionary<string, Expression<Func<T, bool>>> Filters => Facets
            .SelectMany(f => f.Values)
            .ToDictionary(v => v.Key, v => v.Filter);

        public List<Facet<T>> Facets => _facets;

        /// <summary>
        /// Retrieves a dictionary of facet categories and their corresponding filter keys.
        /// </summary>
        /// <returns>A dictionary where keys are facet names and values are lists of filter keys.</returns>
        public Dictionary<string, List<string>> GetFacetKeys()
        {
            return Facets.ToDictionary(f => f.Name, f => f.Values.Select(v => v.Key).ToList());
        }

        /// <summary>
        /// Retrieves the filter expression associated with a given filter key.
        /// </summary>
        /// <param name="key">The filter key.</param>
        /// <returns>The corresponding filter expression.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key does not exist.</exception>
        public Expression<Func<T, bool>> GetFilter(string key)
        {
            return Filters[key];
        }
    }
}
