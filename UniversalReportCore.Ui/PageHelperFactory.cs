using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using UniversalReportCore.Ui;

namespace UniversalReportCore.Ui
{
    public class PageHelperFactory : ReportPageHelperFactoryBase, IReportPageHelperFactory
    {
        public PageHelperFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _helperMap = new Dictionary<string, (Type, Type)>();
        }
    }
}
