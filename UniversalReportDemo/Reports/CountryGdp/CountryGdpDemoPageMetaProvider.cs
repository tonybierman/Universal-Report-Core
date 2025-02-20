using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportDemo.Reports;

namespace UniversalReportDemo.Reports.CountryGdp
{
    public class CountryGdpDemoPageMetaProvider : BaseReportPageMetaProvider, IPageMetaProvider 
    {
        public string Slug => "CountryGdpDemo";

        public override string CategorySlug => "NationalGdpReports";

        public PageMetaViewModel GetPageMeta()
        {
            return new PageMetaViewModel() { Title = "Demo", Subtitle = "Most Recent National GDP" };
        }
    }
}
