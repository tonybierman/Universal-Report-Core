using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class DateRangeFilterStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            if (parameters.DateFilter != null && parameters.DateFilter.HasValue)
            {
                string[] propertyParts = parameters.DateFilter.PropertyName.Split('.');
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "e");

                Expression propertyAccess = param;

                foreach (var part in propertyParts)
                {
                    PropertyInfo? propertyInfo = propertyAccess.Type.GetProperty(part);
                    if (propertyInfo == null) throw new ArgumentException("Invalid property name.");
                    propertyAccess = Expression.Property(propertyAccess, part);
                    // Check if the final property is DateTime, intermediate properties can be objects
                    if (part == propertyParts.Last() && propertyAccess.Type != typeof(DateTime))
                        throw new ArgumentException("Property must be DateTime.");
                }

                if (propertyAccess != param) // Ensure we have a valid property path
                {
                    var startDate = parameters.DateFilter.StartDate != DateTime.MinValue ? parameters.DateFilter.StartDate : DateTime.MinValue;
                    var endDate = parameters.DateFilter.EndDate != DateTime.MinValue ? parameters.DateFilter.EndDate : DateTime.MaxValue;

                    Expression? startExpr = startDate != DateTime.MinValue ? Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(startDate)) : null;
                    Expression? endExpr = endDate != DateTime.MaxValue ? Expression.LessThanOrEqual(propertyAccess, Expression.Constant(endDate)) : null;

                    Expression condition = startExpr != null && endExpr != null
                        ? Expression.AndAlso(startExpr, endExpr)
                        : startExpr ?? endExpr ?? Expression.Constant(true);

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, param);
                    query = query.Where(lambda);
                }
            }
            return query;
        }
    }
}