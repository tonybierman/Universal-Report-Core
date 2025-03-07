using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents the query parameters for a report, enforcing hardening and sanity checks.
    /// </summary>
    public class ReportQueryParamsBase : IReportQueryParams
    {
        /// <summary>
        /// Gets an enumerable collection of all query parameters as <see cref="IHardVariable"/>.
        /// </summary>
        public IEnumerable<IHardVariable> Array => new List<IHardVariable> { Pi, Ipp, SortOrder, CohortIds, Slug };

        /// <summary>
        /// Gets the hardened paging index parameter.
        /// </summary>
        public HardenedPagingIndex Pi { get; }

        /// <summary>
        /// Gets the hardened items-per-page parameter.
        /// </summary>
        public HardenedItemsPerPage Ipp { get; }

        /// <summary>
        /// Gets or sets the hardened column sorting parameter.
        /// </summary>
        public HardenedColumnSort SortOrder { get; set; }

        /// <summary>
        /// Gets the hardened cohort identifier parameter.
        /// </summary>
        public HardenedCohortIdentifiers CohortIds { get; }

        /// <summary>
        /// Gets the hardened report slug parameter.
        /// </summary>
        public HardenedReportSlug Slug { get; }

        /// <summary>
        /// Gets the hardened filter keys parameter.
        /// </summary>
        public HardenedFilterKeys FilterKeys { get; set; }

        /// <summary>
        /// Gets a value indicating whether all query parameters are hardened.
        /// </summary>
        public bool IsHard => Array.All(a => a?.IsHard ?? false);

        /// <summary>
        /// Gets a value indicating whether all query parameters are considered sane.
        /// </summary>
        public bool IsSane => Array.All(a => a?.IsSane ?? false);

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportQueryParamsBase"/> class.
        /// </summary>
        /// <param name="pi">The paging index parameter.</param>
        /// <param name="ipp">The items-per-page parameter.</param>
        /// <param name="sortOrder">The column sorting parameter.</param>
        /// <param name="cohortIds">The cohort identifiers parameter.</param>
        /// <param name="slug">The report slug parameter.</param>
        public ReportQueryParamsBase(HardenedPagingIndex pi,
            HardenedItemsPerPage ipp,
            HardenedColumnSort sortOrder,
            HardenedCohortIdentifiers cohortIds,
            HardenedReportSlug slug,
            HardenedFilterKeys filterKeys)
        {
            Pi = pi;
            Ipp = ipp;
            SortOrder = sortOrder;
            CohortIds = cohortIds;
            Slug = slug;
            FilterKeys = filterKeys;
        }

        /// <summary>
        /// Performs a sanity check on all query parameters.
        /// </summary>
        /// <returns><c>true</c> if all parameters pass the sanity check; otherwise, <c>false</c>.</returns>
        public bool CheckSanity()
        {
            bool allSane = true;
            foreach (var item in Array)
            {
                if (item?.CheckSanity() == false)
                {
                    allSane = false;
                }
            }

            return allSane;
        }
    }
}
