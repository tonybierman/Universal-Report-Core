using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopulationDemoQueryProvider : PagedCityPopulationQueryProvider
    {
        public override string Slug => "CityPopulationDemo";

        public CityPopulationDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
