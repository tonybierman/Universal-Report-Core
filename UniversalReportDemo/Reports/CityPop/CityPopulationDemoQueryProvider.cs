using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationDemoQueryProvider : PagedCityPopulationQueryProvider
    {
        public override string Slug => "CityPopulationDemo";

        public CityPopulationDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
