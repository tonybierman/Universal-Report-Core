using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Defines a provider for generating paged queries with optional cohort-based aggregation and filtering.
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    public interface IPagedQueryProvider<T> where T : class
    {
        /// <summary>
        /// Gets the unique slug identifier for this query provider.
        /// </summary>
        string Slug { get; }

        /// <summary>
        /// Gets the base query for the report
        /// </summary>
        /// <returns></returns>
        IQueryable<T>? EnsureReportQuery();

        /// <summary>
        /// Creates a paged query with sorting, filtering, and cohort selection applied.
        /// </summary>
        /// <param name="columns">The columns to be included in the query.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="sort">The sorting criteria.</param>
        /// <param name="ipp">The number of items per page.</param>
        /// <param name="cohortIds">An array of cohort IDs to filter the query.</param>
        /// <param name="reportQuery">Base query or null for full DBSet<typeparamref name="T"/></param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> object containing the constructed query parameters.</returns>
        PagedQueryParameters<T> BuildPagedQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, IQueryable<T>? reportQuery = null);

        /// <summary>
        /// Modifies the given query to include aggregate computations based on cohort identifiers.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <param name="cohortIds">An array of cohort IDs to aggregate data by.</param>
        /// <returns>The modified query with aggregation logic applied.</returns>
        IQueryable<T> EnsureAggregatePredicate(IQueryable<T> query, int[]? cohortIds);

        /// <summary>
        /// Filters the given query to include only results related to a specific cohort ID.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <param name="cohortId">The cohort ID used to filter the results.</param>
        /// <returns>The filtered query containing only data relevant to the specified cohort.</returns>
        IQueryable<T> EnsureCohortPredicate(IQueryable<T> query, int cohortId);
    }
}
