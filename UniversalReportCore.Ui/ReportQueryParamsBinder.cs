using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;

namespace UniversalReportCore.Ui
{
    public class ReportQueryParamsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var httpContext = bindingContext.HttpContext;
            var query = httpContext.Request.Query;
            var routeData = httpContext.GetRouteData();  // Retrieve route data

            var slug = routeData.Values["slug"]?.ToString();  // Get slug from route data
            var sku = routeData.Values["sku"]?.ToString();  // Get slug from route data

            var model = new ReportQueryParams
            (
                new HardenedPagingIndex(ConvertToNullableInt(query["Pi"])),
                new HardenedItemsPerPage(ConvertToNullableInt(query["Ipp"])),
                new HardenedColumnSort(query["SortOrder"]),
                new HardenedCohortIdentifiers(query["CohortIds"].Select(int.Parse).ToArray()),
                new HardenedReportSlug(slug)  // Use the slug from route data
            );

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private int? ConvertToNullableInt(string value) => int.TryParse(value, out var result) ? result : null;
    }
}
