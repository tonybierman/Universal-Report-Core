using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Represents the query parameters for paginated data retrieval in reports.
    /// This class provides functionality for filtering, sorting, aggregation, 
    /// and metadata retrieval of dataset queries.
    /// </summary>
    /// <typeparam name="T">The entity type for the paged query.</typeparam>
    public class PagedQueryParameters<T> : PagedQueryParametersBase where T : class
    {
        /// <summary>
        /// Gets the report columns definition used in the query.
        /// </summary>
        public IReportColumnDefinition[] ReportColumns { get; set; }

        /// <summary>
        /// Gets or sets an optional additional filter function that applies faceted browsing logic to the query.
        /// </summary>
        public Func<IQueryable<T>, IQueryable<T>>? FacetedFilter { get; set; }

        /// <summary>
        /// Gets or sets the filter function that applies report-specific filtering to the query.
        /// </summary>
        public Func<IQueryable<T>, IQueryable<T>>? ReportFilter { get; set; }

        /// <summary>
        /// Gets or sets an optional function for computing aggregate values (sum, count, average, etc.) on the query results.
        /// Returns a dictionary where keys are column names and values are aggregated results.
        /// </summary>
        public Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? AggregateLogic { get; set; }

        /// <summary>
        /// Gets or sets an optional function for computing metadata related to the query results.
        /// Metadata might include total record count, available filters, etc.
        /// </summary>
        public Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? MetaLogic { get; set; }

        /// <summary>
        /// Gets or sets an optional function that applies cohort-based filtering logic to the query.
        /// Used when filtering by specific cohort identifiers.
        /// </summary>
        public Func<IQueryable<T>, int[], IQueryable<T>>? CohortLogic { get; set; }

        /// <summary>
        /// Gets or sets keys for use in user filtering.
        /// </summary>
        public FilterConfig<T>? FilterConfig { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedQueryParameters{T}"/> class.
        /// </summary>
        /// <param name="columns">The columns included in the report.</param>
        /// <param name="pageIndex">The page index for pagination.</param>
        /// <param name="sort">The sorting order.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="cohortIds">An array of cohort identifiers for filtering.</param>
        /// <param name="filterKeys">A string array of keys for filtering.</param>
        /// <param name="reportFilter">An optional additional filter function.</param>
        /// <param name="aggregateLogic">An optional aggregation function.</param>
        /// <param name="metaLogic">An optional function for computing metadata.</param>
        public PagedQueryParameters(
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? itemsPerPage,
            int[]? cohortIds,
            FilterConfig<T>? filterConfig,
            Func<IQueryable<T>, IQueryable<T>>? reportFilter = null,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? aggregateLogic = null,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? metaLogic = null)
        {
            ReportColumns = columns;
            PageIndex = pageIndex;
            Sort = sort;
            ItemsPerPage = itemsPerPage;
            CohortIds = cohortIds;
            FilterConfig = filterConfig;
            ReportFilter = reportFilter;
            AggregateLogic = aggregateLogic;
            MetaLogic = metaLogic;
        }
    }
}
