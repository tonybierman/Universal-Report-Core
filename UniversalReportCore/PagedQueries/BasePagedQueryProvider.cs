using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductionPlanner.PagedQueries
{
    public abstract class BasePagedQueryProvider<T> : IPagedQueryProvider<T> where T : class
    {
        public abstract string Slug { get; }

        public BasePagedQueryProvider()
        {
        }

        protected virtual async Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<T> query, IReportColumnDefinition[] columns)
        {
            var aggregateResults = new Dictionary<string, dynamic>();

            foreach (var column in columns)
            {
                var propertyName = column.PropertyName;
                var property = typeof(T).GetProperty(propertyName);

                if (property == null)
                {
                    continue; // Skip if the property does not exist
                }

                Type propertyType = property.PropertyType;

                switch (column.Aggregation)
                {
                    case AggregationType.Sum:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            aggregateResults[propertyName] = await query.SumAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                        {
                            aggregateResults[propertyName] = await query.SumAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        }
                        break;

                    case AggregationType.Average:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            aggregateResults[propertyName] = await query.AverageAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                        {
                            aggregateResults[propertyName] = await query.AverageAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        }
                        break;

                    case AggregationType.Min:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            aggregateResults[propertyName] = await query.MinAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                        {
                            aggregateResults[propertyName] = await query.MinAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        }
                        break;

                    case AggregationType.Max:
                        if (propertyType == typeof(int) || propertyType == typeof(int?))
                        {
                            aggregateResults[propertyName] = await query.MaxAsync(x => EF.Property<int?>(x, propertyName)) ?? 0;
                        }
                        else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                        {
                            aggregateResults[propertyName] = await query.MaxAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        }
                        break;

                    case AggregationType.Count:
                        aggregateResults[propertyName] = await query.CountAsync(x => EF.Property<object>(x, propertyName) != null);
                        break;

                    case AggregationType.None:
                    default:
                        aggregateResults[propertyName] = null;
                        break;
                }
            }

            return aggregateResults;
        }

        protected abstract Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<T> query);
        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query)
        {
            return query; // Default: No filtering
        }
        public virtual IQueryable<T> EnsureAggregateQuery(IQueryable<T> query, int[]? cohortIds)
        {
            return query; // Default: No filtering
        }
        public virtual IQueryable<T> EnsureCohortQuery(IQueryable<T> query, int cohortId)
        {
            return query; // Default: No filtering
        }
        public PagedQueryParameters<T> GetQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return new PagedQueryParameters<T>(
                columns,
                pageIndex,
                sort,
                ipp,
                cohortIds,
                query => ApplyFilters((IQueryable<T>)query),  // Corrected to handle generic type T
                async (IQueryable<T> src) =>
                {
                    var aggregates = new Dictionary<string, dynamic>();
                    var filteredQuery = ApplyFilters(src);

                    if (cohortIds == null || cohortIds.Length == 0)
                    {
                        return await ComputeAggregates(filteredQuery, columns);
                    }

                    var totalAggregates = await ComputeAggregates(EnsureAggregateQuery(filteredQuery, cohortIds), columns);

                    foreach (var key in totalAggregates.Keys)
                    {
                        aggregates[$"{key}"] = totalAggregates[key];
                    }

                    foreach (var cohortId in cohortIds)
                    {
                        var cohortQuery = EnsureCohortQuery(filteredQuery, cohortId);
                        var cohortResults = await ComputeAggregates(cohortQuery, columns);
                        foreach (var key in cohortResults.Keys)
                        {
                            aggregates[$"{key}_{cohortId}"] = cohortResults[key];
                        }
                    }

                    return aggregates;
                },
                async (IQueryable<T> src) =>
                {
                    var meta = new Dictionary<string, dynamic>();
                    var filteredQuery = ApplyFilters(src);

                    var metaDictionary = await EnsureMeta(filteredQuery);
                    foreach (var key in metaDictionary.Keys)
                    {
                        meta[$"{key}"] = metaDictionary[key];
                    }

                    return meta;
                }
            );
        }
    }
}
