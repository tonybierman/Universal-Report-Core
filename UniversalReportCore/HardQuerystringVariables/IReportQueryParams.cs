using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.HardQuerystringVariables
{
    public interface IReportQueryParams
    {
        IEnumerable<IHardVariable> Array { get; }
        HardenedCohortIdentifiers CohortIds { get; }
        HardenedItemsPerPage Ipp { get; }
        bool IsHard { get; }
        bool IsSane { get; }
        HardenedPagingIndex Pi { get; }
        HardenedReportSlug Slug { get; }
        HardenedColumnSort SortOrder { get; set; }

        bool CheckSanity();
    }
}