using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CountryGdp
{
    public class CountryGdpDemoQueryProvider : PagedNationalGdpQueryProvider
    {
        public override string Slug => "CountryGdpDemo";

        public CountryGdpDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
