using UniversalReportCore;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportHeavyDemo.Reports;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopDemoPageMetaProvider : BaseReportPageMetaProvider, IPageMetaProvider 
    {
        public string Slug => "CityPopDemo";

        public override string CategorySlug => "CityPopulationReports";

        public PageMetaViewModel GetPageMeta()
        {
            return new PageMetaViewModel() { Title = "Demo", Subtitle = "Most Recent City Populations" };
        }
    }
}
