using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class SearchFilterStep<TEntity> : IPipelineStage<TEntity>, IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> ApplySearchFilters(IQueryable<TEntity> query, TextFilter[]? searchFilters)
        {
            // Check if search filters exist and are not empty
            if (searchFilters != null && searchFilters.Any())
            {
                // Group filters by PropertyName for OR logic within same property
                var groups = searchFilters
                    .Where(f => !string.IsNullOrEmpty(f.Value))
                    .GroupBy(f => f.PropertyName)
                    .ToList();

                if (groups.Any())
                {
                    // Define parameter for expression
                    ParameterExpression param = Expression.Parameter(typeof(TEntity), "e");
                    Expression? overallCondition = null;

                    // Process each property group
                    foreach (var group in groups)
                    {
                        // Split property path for nested properties
                        string[] propertyParts = group.Key.Split('.');
                        Expression propertyAccess = param;

                        // Build property access expression
                        foreach (var part in propertyParts)
                        {
                            PropertyInfo? propertyInfo = propertyAccess.Type.GetProperty(part);
                            if (propertyInfo == null) throw new ArgumentException("Invalid property name.");
                            propertyAccess = Expression.Property(propertyAccess, part);
                            // Validate final property is string
                            if (part == propertyParts.Last() && propertyAccess.Type != typeof(string))
                                throw new ArgumentException("Property must be string.");
                        }

                        // Ensure valid property path
                        if (propertyAccess != param)
                        {
                            // Get string.Contains method for comparison
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                            Expression? groupExpr = null;

                            // Combine filters for same property with OR
                            foreach (var filter in group)
                            {
                                Expression condition = Expression.Call(propertyAccess, containsMethod, Expression.Constant(filter.Value));
                                groupExpr = groupExpr == null ? condition : Expression.OrElse(groupExpr, condition);
                            }

                            // Combine different properties with AND
                            if (groupExpr != null)
                            {
                                overallCondition = overallCondition == null ? groupExpr : Expression.AndAlso(overallCondition, groupExpr);
                            }
                        }
                    }

                    // Apply combined filter expression to query
                    if (overallCondition != null)
                    {
                        var lambda = Expression.Lambda<Func<TEntity, bool>>(overallCondition, param);
                        query = query.Where(lambda);
                    }
                }
            }
            // Return filtered or original query
            return query;
        }

        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            return ApplySearchFilters(query, parameters.SearchFilters);
        }

        public Task<PipelineResult<TEntity>> ExecuteAsync(PipelineResult<TEntity> input)
        {
            input.Query = ApplySearchFilters(input.Query, input.SearchFilters); // Returns 

            return Task.FromResult(input);
        }
    }
}