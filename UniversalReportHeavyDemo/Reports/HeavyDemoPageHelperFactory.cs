using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using UniversalReportCore.Ui;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace UniversalReportHeavyDemo.Reports
{
    public class HeavyDemoPageHelperFactory : PageHelperFactory, IReportPageHelperFactory
    {
        public HeavyDemoPageHelperFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _helperMap = new Dictionary<string, (Type, Type)>
            {
                { "CityPopulationReports", (typeof(CityPopulation), typeof(CityPopulationViewModel)) }
            };
        }
    }
}
