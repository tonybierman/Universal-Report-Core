// Filename: BasePagedQueryProvider.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Base class for paged query providers, refactored to use a pipeline pattern.
    /// </summary>
    public abstract class BasePagedQueryProvider<T> : IPagedQueryProvider<T> where T : class
    {
        public abstract string Slug { get; }

        public BasePagedQueryProvider() { }

        // TODO: Column aggregates don't account for faceted filters
        public virtual async Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<T> query, IReportColumnDefinition[] columns)
        {
            var aggregateResults = new Dictionary<string, dynamic>();

            foreach (var column in columns)
            {
                var propertyName = column.PropertyName;
                if (string.IsNullOrEmpty(propertyName))
                    continue;

                var property = typeof(T).GetProperty(propertyName);
                if (property == null)
                    continue;

                Type propertyType = property.PropertyType;

                var query1 = EnsureUserFiltersPredicate(query);

                switch (column.Aggregation)
                {
                    case AggregationType.Sum:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                            aggregateResults[propertyName] = await query1.SumAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                            aggregateResults[propertyName] = await query1.SumAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Average:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                            aggregateResults[propertyName] = await query1.AverageAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                            aggregateResults[propertyName] = await query1.AverageAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Min:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                            aggregateResults[propertyName] = await query1.MinAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                            aggregateResults[propertyName] = await query1.MinAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Max:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                            aggregateResults[propertyName] = await query1.MaxAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                            aggregateResults[propertyName] = await query1.MaxAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Count:
                        aggregateResults[propertyName] = await query1.CountAsync(x => EF.Property<object>(x, propertyName) != null);
                        break;

                    case AggregationType.StandardDeviation:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            var values = await query1.Select(x => EF.Property<int?>(x, propertyName)).Where(x => x != null).Cast<double>().ToListAsync();
                            aggregateResults[propertyName] = values.Any() ? Math.Sqrt(values.Average(v => Math.Pow(v - values.Average(), 2))) : 0;
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                        {
                            var values = await query1.Select(x => EF.Property<decimal?>(x, propertyName)).Where(x => x != null).Cast<double>().ToListAsync();
                            aggregateResults[propertyName] = values.Any() ? (decimal)Math.Sqrt(values.Average(v => Math.Pow(v - values.Average(), 2))) : 0;
                        }
                        break;

                    case AggregationType.None:
                    default:
                        aggregateResults[propertyName] = null;
                        break;
                }
            }

            return aggregateResults;
        }

        public virtual IQueryable<T>? EnsureReportQuery()
        {
            throw new NotImplementedException("This report's query provider does not implement an EnsureReportQuery() method.");
        }

        protected abstract Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<T> query);

        protected virtual IQueryable<T> EnsureUserFiltersPredicate(IQueryable<T> query)
        {
            return query;
        }

        public virtual IQueryable<T> EnsureAggregatePredicate(IQueryable<T> query, int[]? cohortIds)
        {
            return query;
        }

        public virtual IQueryable<T> EnsureCohortPredicate(IQueryable<T> query, int cohortId)
        {
            return query;
        }

        // TODO: Facet filters need to be passed in here
        public virtual PagedQueryParameters<T> BuildPagedQuery(
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? ipp,
            int[]? cohortIds,
            IQueryable<T>? reportQuery = null)
        {

            var pipeline = new QueryPipeline<T>()
                .AddStage(new UserFilterStage<T>(EnsureUserFiltersPredicate))
                .AddStage(new CohortAggregateStage<T>(columns, cohortIds, ComputeAggregates, EnsureCohortPredicate, EnsureAggregatePredicate))
                .AddStage(new MetadataStage<T>(EnsureMeta));

            var query = reportQuery ?? EnsureReportQuery() ?? throw new InvalidOperationException("No query provided");

            return pipeline.ExecuteAsync(query, columns, pageIndex, sort, ipp, cohortIds).Result;
        }
    }
}