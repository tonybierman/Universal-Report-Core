using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class SearchFilterStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            if (parameters.SearchFilter != null && !string.IsNullOrEmpty(parameters.SearchFilter.Value))
            {
                string[] propertyParts = parameters.SearchFilter.PropertyName.Split('.');
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "e");

                Expression propertyAccess = param;

                foreach (var part in propertyParts)
                {
                    PropertyInfo propertyInfo = propertyAccess.Type.GetProperty(part);
                    if (propertyInfo == null) throw new ArgumentException("Invalid property name.");
                    propertyAccess = Expression.Property(propertyAccess, part);
                    if (part == propertyParts.Last() && propertyAccess.Type != typeof(string))
                        throw new ArgumentException("Property must be string.");
                }

                if (propertyAccess != param)
                {
                    var searchValue = parameters.SearchFilter.Value;
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    Expression condition = Expression.Call(propertyAccess, containsMethod, Expression.Constant(searchValue));

                    var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, param);
                    query = query.Where(lambda);
                }
            }
            return query;
        }
    }
}