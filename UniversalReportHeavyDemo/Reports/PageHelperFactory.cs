using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace UniversalReportHeavyDemo.Reports
{
    public class PageHelperFactory : ReportPageHelperFactoryBase
    {
        public PageHelperFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _helperMap = new Dictionary<string, (Type, Type)>
            {
                { "CityPopulationReports", (typeof(CityPopulation), typeof(CityPopulationViewModel)) },
                { "NationalGdpReports", (typeof(NationalGdp), typeof(NationalGdpViewModel)) }
            };
        }
    }
}
