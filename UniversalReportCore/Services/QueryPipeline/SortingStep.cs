using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.Helpers;
using UniversalReportCore.PagedQueries;
using System.Linq.Dynamic.Core;
namespace UniversalReportCore.Services.QueryPipeline
{
    public class SortingStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            if (!string.IsNullOrEmpty(parameters.Sort))
            {
                var sortOrder = parameters.Sort.Replace("{", "").Replace("}", "");
                var isDescending = SortHelper.IsDescending(sortOrder);
                var baseSortKey = SortHelper.BaseSortKey(sortOrder);
                var column = parameters.ReportColumns.FirstOrDefault(c => c.PropertyName == baseSortKey);

                if (column != null && column.IsSortable)
                {
                    // Use System.Linq.Dynamic.Core to apply dynamic sorting
                    string orderByExpression = $"{column.PropertyName} {(isDescending ? "descending" : "ascending")}";
                    query = query.OrderBy(orderByExpression); // Ensure this uses Dynamic.Core
                }
            }
            return query;
        }
    }
}
