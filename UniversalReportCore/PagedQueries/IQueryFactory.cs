using System;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Defines a contract for a factory that creates paged query parameters for a specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type for which query parameters are created.</typeparam>
    public interface IQueryFactory<T> where T : class
    {
        /// <summary>
        /// Creates paged query parameters for a given query type (slug).
        /// </summary>
        /// <param name="queryType">The identifier of the query type (slug).</param>
        /// <param name="columns">An array of column definitions to be used in the query.</param>
        /// <param name="pageIndex">The page index for pagination.</param>
        /// <param name="sort">The sorting criteria for the query.</param>
        /// <param name="ipp">The number of items per page.</param>
        /// <param name="cohortIds">An array of cohort IDs to filter the data.</param>
        /// <param name="filterKeys">A string array of keys to filter the data.</param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> instance containing the query parameters.</returns>
        PagedQueryParameters<T> CreateQueryParameters(
            string queryType,
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? ipp,
            int[]? cohortIds,
            string[]? filterKeys);
    }
}
