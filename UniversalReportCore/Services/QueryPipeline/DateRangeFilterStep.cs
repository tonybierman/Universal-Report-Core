using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.PagedQueries;
using System.Linq.Expressions;
using System.Reflection;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class DateRangeFilterStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        /// <summary>
        /// Executes a query with date filtering, supporting dot notation for nested properties.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="query">The input IQueryable to filter.</param>
        /// <param name="parameters">Query parameters including date filter.</param>
        /// <returns>Filtered IQueryable based on date range.</returns>
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            if (parameters.DateFilter != null && parameters.DateFilter.HasValue)
            {
                string[] propertyParts = parameters.DateFilter.PropertyName.Split('.');
                PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyParts[0]);

                if (propertyParts.Length == 1 && propertyInfo != null)
                {
                    query = query.Where(e => EF.Property<DateTime>(e, propertyParts[0]) >= parameters.DateFilter.StartDate)
                                .Where(e => EF.Property<DateTime>(e, propertyParts[0]) <= parameters.DateFilter.EndDate);
                }
                else if (propertyParts.Length > 1)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    Expression propertyAccess = parameter;

                    foreach (var part in propertyParts)
                    {
                        propertyAccess = Expression.Property(propertyAccess, part);
                    }

                    var startDateLambda = Expression.Lambda<Func<TEntity, bool>>(
                        Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(parameters.DateFilter.StartDate)),
                        parameter);

                    var endDateLambda = Expression.Lambda<Func<TEntity, bool>>(
                        Expression.LessThanOrEqual(propertyAccess, Expression.Constant(parameters.DateFilter.EndDate)),
                        parameter);

                    query = query.Where(startDateLambda).Where(endDateLambda);
                }
            }
            return query;
        }
    }
}
