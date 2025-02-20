using UniversalReportCore.PagedQueries;
using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationQueryFactory : QueryFactory<CityPopulation>
    {
        public CityPopulationQueryFactory(IEnumerable<IPagedQueryProvider<CityPopulation>> providers) : base(providers)
        {
        }
    }
}
