using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
{
    public class CountryGdpDemoQueryProvider : PagedNationalGdpQueryProvider
    {
        public override string Slug => "CountryGdpDemo";

        public CountryGdpDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
