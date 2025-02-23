using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
{
    public class PagedNationalGdpQueryParameters : PagedQueryParameters<NationalGdp>
    {
        public PagedNationalGdpQueryParameters(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? itemsPerPage, int[]? cohortIds,
            Func<IQueryable<NationalGdp>, IQueryable<NationalGdp>>? additionalFilter = null,
            Func<IQueryable<NationalGdp>, Task<Dictionary<string, dynamic>>>? aggregateLogic = null,
            Func<IQueryable<NationalGdp>, Task<Dictionary<string, dynamic>>>? metaLogic = null) :
            base(columns, pageIndex, sort, itemsPerPage, cohortIds, additionalFilter, aggregateLogic, metaLogic)
        {
        }
    }
}
