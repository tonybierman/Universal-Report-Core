using UniversalReportCore;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportHeavyDemo.Reports;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopulationDemoPageMetaProvider : BaseReportPageMetaProvider, IPageMetaProvider 
    {
        public string Slug => "CityPopulationDemo";

        public override string CategorySlug => "CityPopulationReports";

        public PageMetaViewModel GetPageMeta()
        {
            return new PageMetaViewModel() { Title = "Demo", Subtitle = "Most Recent City Populations" };
        }
    }
}
