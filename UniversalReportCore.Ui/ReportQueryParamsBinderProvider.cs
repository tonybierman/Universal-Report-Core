using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui
{

    public class ReportQueryParamsBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(IReportQueryParamsBase))
            {
                return new BinderTypeModelBinder(typeof(ReportQueryParamsBinderBase));
            }

            return null;  // Return null to let the default binders handle other types
        }
    }
}
