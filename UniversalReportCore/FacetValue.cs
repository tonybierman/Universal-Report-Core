using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Represents a single filterable value within a facet.
    /// </summary>
    /// <typeparam name="T">The type of entity being filtered.</typeparam>
    public class FacetValue<T>
    {
        /// <summary>
        /// Gets the unique key identifying this facet value.
        /// </summary>
        public string Key { get; }

        public string DisplayName { get; set; } // UI-friendly name (e.g., "Canada", "Male")

        /// <summary>
        /// Gets the filter expression associated with this facet value.
        /// </summary>
        public Expression<Func<T, bool>> Filter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacetValue{T}"/> class.
        /// </summary>
        /// <param name="key">The unique key representing this facet value.</param>
        /// <param name="filter">The filter expression used to filter data based on this facet value.</param>
        /// <param name="displayName">Optional display name for the facet value. Defaults to the key if not provided.</param>
        public FacetValue(string key, Expression<Func<T, bool>> filter, string? displayName = null)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
            DisplayName = displayName ?? key;
        }
    }
}
