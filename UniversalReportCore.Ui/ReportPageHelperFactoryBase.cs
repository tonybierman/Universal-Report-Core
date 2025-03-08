using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui
{
    public class ReportPageHelperFactoryBase
    {
        protected readonly IServiceProvider _serviceProvider;
        protected Dictionary<string, (Type EntityType, Type ViewModelType)> _helperMap = new Dictionary<string, (Type EntityType, Type ViewModelType)>();

        public ReportPageHelperFactoryBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReportPageHelperBase GetHelper(string reportType)
        {
            if (!_helperMap.TryGetValue(reportType, out var types))
            {
                throw new ArgumentException($"No PageHelper found for report type {reportType}");
            }

            var helperType = typeof(IReportPageHelper<,>).MakeGenericType(types.EntityType, types.ViewModelType);
            var helperInstance = _serviceProvider.GetService(helperType);

            return helperInstance as IReportPageHelperBase
                ?? throw new InvalidOperationException($"Failed to resolve {helperType.Name}");
        }
    }
}
