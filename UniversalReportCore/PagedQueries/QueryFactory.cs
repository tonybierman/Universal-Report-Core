using System;
using System.Collections.Generic;
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
        /// <param name="slug">The unique identifier of the query type.</param>
        /// <param name="columns">An array of column definitions to be used in the query.</param>
        /// <param name="pageIndex">The page index for pagination.</param>
        /// <param name="sort">The sorting criteria for the query.</param>
        /// <param name="ipp">Items per page.</param>
        /// <param name="cohortIds">An array of cohort IDs to filter the data.</param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> instance containing the query parameters.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the specified slug does not match any available query provider.</exception>
        public PagedQueryParameters<T> CreateQueryParameters(
            string slug,
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? ipp,
            int[]? cohortIds)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported query type: {slug}");
            }

            return provider.BuildPagedQuery(columns, pageIndex, sort, ipp, cohortIds, provider.EnsureReportQuery());
        }
    }
}
