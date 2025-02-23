using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportHeavyDemo.Reports;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
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
