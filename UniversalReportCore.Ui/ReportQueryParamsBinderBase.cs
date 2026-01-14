using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using System.Collections.Generic;
using System.Linq;

namespace UniversalReportCore.Ui
{
    /// <summary>
    /// Model binder for IReportQueryParams that safely converts query strings and route data.
    /// Returns ReportQueryParams.None when binding fails or required data is missing.
    /// </summary>
    public class ReportQueryParamsBinderBase : IModelBinder
    {
        public virtual Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var httpContext = bindingContext.HttpContext;
                var query = httpContext.Request.Query;
                var routeData = httpContext.GetRouteData();
                
                var slug = routeData?.Values["slug"]?.ToString();
                var sku = routeData?.Values["sku"]?.ToString();

                // Only attempt binding if we have at least a slug
                if (string.IsNullOrEmpty(slug))
                {
                    bindingContext.Result = ModelBindingResult.Success(ReportQueryParams.None);
                    return Task.CompletedTask;
                }

                var searchDict = query
                    .Where(kvp => kvp.Key.StartsWith("query") && !string.IsNullOrEmpty(kvp.Value))
                    .ToDictionary(kvp => kvp.Key.Substring("query".Length), kvp => kvp.Value.ToString());

                var model = new ReportQueryParams
                (
                    new HardenedPagingIndex(ConvertToNullableInt(query["Pi"].ToString())),
                    new HardenedItemsPerPage(ConvertToNullableInt(query["Ipp"].ToString())),
                    new HardenedColumnSort(query["SortOrder"].ToString() ?? string.Empty),
                    new HardenedCohortIdentifiers(query["CohortIds"].Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray()),
                    new HardenedReportSlug(slug),
                    new HardenedFilterKeys(query["Filters"].Where(s => s != null).Cast<string>().ToArray()),
                    new HardenedSearchQueries(searchDict)
                );
                
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch
            {
                // On any binding error, return the None singleton instead of throwing
                bindingContext.Result = ModelBindingResult.Success(ReportQueryParams.None);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Safely converts a string to nullable int.
        /// </summary>
        private int? ConvertToNullableInt(string? value) => 
            int.TryParse(value, out var result) ? result : null;
    }
}
