using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Reports.Domain;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopDemoQueryProvider : PagedCityPopulationQueryProvider
    {
        public override string Slug => "CityPopDemo";

        public CityPopDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
