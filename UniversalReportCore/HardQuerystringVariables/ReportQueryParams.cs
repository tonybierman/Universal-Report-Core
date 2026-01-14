using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents the query parameters for a report, providing a concrete implementation with null object pattern support.
    /// Use the <see cref="None"/> singleton to represent missing or invalid query parameters throughout your application.
    /// </summary>
    /// <example>
    /// <code>
    /// public IActionResult MyReport(IReportQueryParams queryParams)
    /// {
    ///     if (queryParams == ReportQueryParams.None)
    ///     {
    ///         return BadRequest("Missing required report parameters");
    ///     }
    ///     // Process valid parameters
    /// }
    /// </code>
    /// </example>
    public class ReportQueryParams : ReportQueryParamsBase
    {
        /// <summary>
        /// Gets the "none" singleton instance representing absence of query parameters.
        /// This is a thread-safe, reusable instance that can be used for reference comparison and default values.
        /// </summary>
        /// <remarks>
        /// Use this instead of null to avoid null reference exceptions throughout your application.
        /// All properties on this instance return Default values for their respective hardened types.
        /// </remarks>
        public static ReportQueryParams None { get; } = new ReportQueryParams(
            HardenedPagingIndex.Default,
            HardenedItemsPerPage.Default,
            HardenedColumnSort.Default,
            HardenedCohortIdentifiers.Default,
            HardenedReportSlug.Default,
            HardenedFilterKeys.Default,
            HardenedSearchQueries.Default);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportQueryParams"/> class.
        /// All parameters should be properly hardened and pass sanity checks.
        /// </summary>
        /// <param name="pi">The hardened paging index parameter.</param>
        /// <param name="ipp">The hardened items-per-page parameter.</param>
        /// <param name="sortOrder">The hardened column sorting parameter.</param>
        /// <param name="cohortIds">The hardened cohort identifiers parameter.</param>
        /// <param name="slug">The hardened report slug parameter.</param>
        /// <param name="filterKeys">The hardened filter keys parameter.</param>
        /// <param name="searchQueries">The hardened search queries parameter.</param>
        public ReportQueryParams(HardenedPagingIndex pi,
            HardenedItemsPerPage ipp,
            HardenedColumnSort sortOrder,
            HardenedCohortIdentifiers cohortIds,
            HardenedReportSlug slug,
            HardenedFilterKeys filterKeys,
            HardenedSearchQueries searchQueries)
            : base(pi, ipp, sortOrder, cohortIds, slug, filterKeys, searchQueries)
        {
        }
    }
}
