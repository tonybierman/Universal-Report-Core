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
        /// <param name="preQueryArgs">Pre-query argument set.</param>
        /// <param name="filterConfig">A configuration to filter the data.</param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> instance containing the query parameters.</returns>
        PagedQueryParameters<T> CreateQueryParameters(
            PreQueryArguments preQueryArgs,
            FilterConfig<T>? filterConfig = null);
    }
}
