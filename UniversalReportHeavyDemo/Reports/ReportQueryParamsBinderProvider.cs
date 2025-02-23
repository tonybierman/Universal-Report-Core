using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportHeavyDemo.Reports
{

    public class ReportQueryParamsBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(ReportQueryParams))
            {
                return new BinderTypeModelBinder(typeof(ReportQueryParamsBinder));
            }

            return null;  // Return null to let the default binders handle other types
        }
    }
}
