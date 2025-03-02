using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using UniversalReportCoreTests.Data;
using UniversalReportCoreTests.ViewModels;

namespace UniversalReportCoreTests.Reports
{
    public class PageHelperFactory : ReportPageHelperFactoryBase, IReportPageHelperFactory
    {
        public PageHelperFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _helperMap = new Dictionary<string, (Type, Type)>
            {
                { "WidgetReports", (typeof(Widget), typeof(WidgetViewModel)) }
            };
        }
    }
}
