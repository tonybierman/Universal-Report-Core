using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportDemo.Reports;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationDemoPageMetaProvider : BaseReportPageMetaProvider, IPageMetaProvider 
    {
        public string Slug => "CityPopulationDemo";

        public override string CategorySlug => "CityPopulationReports";

        public PageMetaViewModel GetPageMeta()
        {
            return new PageMetaViewModel() { Title = "Demo", Subtitle = "City Populations" };
        }
    }
}
