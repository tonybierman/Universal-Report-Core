using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Domain
{
    public class PagedCityPopulationQueryParameters : PagedQueryParameters<CityPopulation>
    {
        public PagedCityPopulationQueryParameters(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? itemsPerPage, int[]? cohortIds,
            Func<IQueryable<CityPopulation>, IQueryable<CityPopulation>>? additionalFilter = null,
            Func<IQueryable<CityPopulation>, Task<Dictionary<string, dynamic>>>? aggregateLogic = null,
            Func<IQueryable<CityPopulation>, Task<Dictionary<string, dynamic>>>? metaLogic = null) :
            base(columns, pageIndex, sort, itemsPerPage, cohortIds, additionalFilter, aggregateLogic, metaLogic)
        {
        }
    }
}
