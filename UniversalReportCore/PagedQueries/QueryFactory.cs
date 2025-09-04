using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// A factory that creates paged query parameters for a specified data type.
    /// </summary>
    /// <typeparam name="T">The entity type for which query parameters are created.</typeparam>
    public class QueryFactory<T> : IQueryFactory<T> where T : class
    {
        private readonly IEnumerable<IPagedQueryProvider<T>> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFactory{T}"/> class.
        /// </summary>
        /// <param name="providers">A collection of query providers that supply query parameters based on a slug.</param>
        public QueryFactory(IEnumerable<IPagedQueryProvider<T>> providers)
        {
            _providers = providers;
        }

        /// <summary>
        /// Creates paged query parameters based on the specified query type (slug).
        /// </summary>
        /// <param name="preQueryArgs">Pre-query argument set.</param>
        /// <param name="filterConfig">A configuration to filter the data, if applicable.</param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> instance containing the query parameters.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified slug does not match any available query provider.</exception>
        public PagedQueryParameters<T> CreateQueryParameters(
            PreQueryArguments preQueryArgs,
            FilterConfig<T>? filterConfig = null)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == preQueryArgs.QueryType);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported query type: {preQueryArgs.QueryType}");
            }

            var retval = provider.BuildPagedQuery(preQueryArgs, filterConfig, provider.EnsureReportQuery());
            retval.FilterKeys = filterConfig?.FilterKeys ?? Array.Empty<string>();

            retval.SearchFilters = preQueryArgs.SearchFilters;

            return retval;
        }
    }
}
