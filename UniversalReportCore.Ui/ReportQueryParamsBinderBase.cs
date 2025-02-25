using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.Ui
{
    public class ReportQueryParamsBinderBase : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var httpContext = bindingContext.HttpContext;
            var query = httpContext.Request.Query;
            var routeData = httpContext.GetRouteData();  // Retrieve route data

            var slug = routeData.Values["slug"]?.ToString();  // Get slug from route data
            var sku = routeData.Values["sku"]?.ToString();  // Get slug from route data

            var model = new ReportQueryParamsBase
            (
                new HardenedPagingIndex(ConvertToNullableInt(query["Pi"])),
                new HardenedItemsPerPage(ConvertToNullableInt(query["Ipp"])),
                new HardenedColumnSort(query["SortOrder"]),
                new HardenedCohortIdentifiers(
                    query.TryGetValue("CohortIds", out StringValues value) && !StringValues.IsNullOrEmpty(value)
                        ? value.Where(v => int.TryParse(v, out _)).Select(int.Parse).ToArray()
                        : Array.Empty<int>()
                ),
                new HardenedReportSlug(slug)  // Use the slug from route data
            );

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private int? ConvertToNullableInt(string value) => int.TryParse(value, out var result) ? result : null;
    }
}
