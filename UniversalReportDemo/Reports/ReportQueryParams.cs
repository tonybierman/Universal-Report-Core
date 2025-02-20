using System.Linq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportDemo.Reports
{
    public class ReportQueryParams
    {
        public IEnumerable<IHardVariable> Array => new List<IHardVariable> { Pi, Ipp, SortOrder, CohortIds, Slug };

        public HardenedPagingIndex Pi { get; }
        public HardenedItemsPerPage Ipp { get; }
        public HardenedColumnSort SortOrder { get; set; }
        public HardenedCohortIdentifiers CohortIds { get; }
        public HardenedReportSlug Slug { get; }

        public bool IsHard => Array.All(a => a?.IsHard ?? false);
        public bool IsSane => Array.All(a => a?.IsSane ?? false);

        public ReportQueryParams(HardenedPagingIndex pi,
            HardenedItemsPerPage ipp,
            HardenedColumnSort sortOrder,
            HardenedCohortIdentifiers cohortIds,
            HardenedReportSlug slug)
        {
            Pi = pi;
            Ipp = ipp;
            SortOrder = sortOrder;
            CohortIds = cohortIds;
            Slug = slug;
        }

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
