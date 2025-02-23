using Microsoft.AspNetCore.Razor.TagHelpers;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace UniversalReportHeavyDemo.Reports
{
    public class PageHelperFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, (Type EntityType, Type ViewModelType)> _helperMap;

        public PageHelperFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _helperMap = new Dictionary<string, (Type, Type)>
        {
            { "CityPopulationReports", (typeof(CityPopulation), typeof(CityPopulationViewModel)) },
            { "NationalGdpReports", (typeof(NationalGdp), typeof(NationalGdpViewModel)) }
        };
        }

        public object GetHelper(string reportType)
        {
            if (!_helperMap.TryGetValue(reportType, out var types))
            {
                throw new ArgumentException($"No PageHelper found for report type {reportType}");
            }

            var helperType = typeof(IReportPageHelper<,>).MakeGenericType(types.EntityType, types.ViewModelType);
            return _serviceProvider.GetService(helperType) ?? throw new InvalidOperationException($"Failed to resolve {helperType.Name}");
        }
    }
}
