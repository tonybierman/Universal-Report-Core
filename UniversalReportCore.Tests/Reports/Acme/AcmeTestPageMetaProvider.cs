using UniversalReportCore;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Tests.Reports.Acme
{
    [PageMetaPolicy("RequireAuthenticated")]
    public class AcmeTestPageMetaProvider : BaseReportPageMetaProvider, IPageMetaProvider
    {
        public string Slug => "CityPopulationDemo";

        public override string CategorySlug => "CityPopulationReports";

        public PageMetaViewModel GetPageMeta()
        {
            return new PageMetaViewModel() { Title = "Demo", Subtitle = "Most Recent City Populations" };
        }
    }
}
