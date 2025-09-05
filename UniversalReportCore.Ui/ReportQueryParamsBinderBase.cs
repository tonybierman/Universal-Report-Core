using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using System.Collections.Generic;
using System.Linq;

namespace UniversalReportCore.Ui
{
    public class ReportQueryParamsBinderBase : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var httpContext = bindingContext.HttpContext;
            var query = httpContext.Request.Query;
            var routeData = httpContext.GetRouteData();
            var slug = routeData.Values["slug"]?.ToString();
            var sku = routeData.Values["sku"]?.ToString();

            var searchDict = query
                .Where(kvp => kvp.Key.StartsWith("query") && !string.IsNullOrEmpty(kvp.Value))
                .ToDictionary(kvp => kvp.Key.Substring("query".Length), kvp => kvp.Value.ToString());

            var model = new ReportQueryParamsBase
            (
                new HardenedPagingIndex(ConvertToNullableInt(query["Pi"])),
                new HardenedItemsPerPage(ConvertToNullableInt(query["Ipp"])),
                new HardenedColumnSort(query["SortOrder"]),
                new HardenedCohortIdentifiers(query["CohortIds"].Select(int.Parse).ToArray()),
                new HardenedReportSlug(slug),
                new HardenedFilterKeys(query["Filters"]),
                new HardenedSearchQueries(searchDict)
            );
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private int? ConvertToNullableInt(string value) => int.TryParse(value, out var result) ? result : null;
    }
}