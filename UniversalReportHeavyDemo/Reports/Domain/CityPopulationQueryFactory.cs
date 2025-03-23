using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Domain
{
    public class CityPopulationQueryFactory : QueryFactory<CityPopulation>
    {
        public CityPopulationQueryFactory(IEnumerable<IPagedQueryProvider<CityPopulation>> providers) : base(providers)
        {
        }
    }
}
