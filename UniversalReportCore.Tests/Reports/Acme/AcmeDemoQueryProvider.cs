using UniversalReportCoreTests.Data;

namespace UniversalReportCore.Tests.Reports.Acme
{
    public class AcmeDemoQueryProvider : PagedAcmeQueryProvider
    {
        public override string Slug => "CityPopulationDemo";

        public AcmeDemoQueryProvider(AcmeDbContext dbContext) : base(dbContext) { }
    }
}
