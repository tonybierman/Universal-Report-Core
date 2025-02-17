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

                // Ensure the property exists on the ProductInventory model
                var property = typeof(T).GetProperty(propertyName);
                if (property == null)
                {
                    continue;
                }

                // Handle the aggregation logic based on the AggregationType
                switch (column.Aggregation)
                {
                    case AggregationType.Sum:
                        aggregateResults[propertyName] = await query.SumAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Average:
                        aggregateResults[propertyName] = await query.AverageAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Min:
                        aggregateResults[propertyName] = await query.MinAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Max:
                        aggregateResults[propertyName] = await query.MaxAsync(x => EF.Property<decimal?>(x, propertyName)) ?? 0;
                        break;

                    case AggregationType.Count:
                        aggregateResults[propertyName] = await query.CountAsync(x => EF.Property<object>(x, propertyName) != null);
                        break;

                    case AggregationType.None:
                    default:
                        // If no aggregation is needed, you can return the original value or leave it out
                        aggregateResults[propertyName] = null;
                        break;
                }
            }

            return aggregateResults;
        }

        //protected abstract Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<T> query, IReportColumnDefinition[] columns);
        //protected override async Task<Dictionary<string, dynamic>> ComputeAggregates(
        //IQueryable<ProductInventory> query,
        //IReportColumnDefinition[] columns)
        //    {
        //        var aggregates = new Dictionary<string, dynamic>();

        //        foreach (var column in columns)
        //        {
        //            var property = typeof(ProductInventory).GetProperty(column.PropertyName);
        //            if (property == null) continue; // Skip if property doesn't exist

        //            switch (column.Aggregation)
        //            {
        //                case AggregationType.Sum:
        //                    aggregates[column.PropertyName] = await query.SumAsync(x =>
        //                        (decimal?)property.GetValue(x) ?? 0);
        //                    break;

        //                case AggregationType.Average:
        //                    aggregates[column.PropertyName] = await query.AverageAsync(x =>
        //                        (decimal?)property.GetValue(x) ?? 0);
        //                    break;

        //                case AggregationType.Min:
        //                    aggregates[column.PropertyName] = await query.MinAsync(x =>
        //                        (decimal?)property.GetValue(x) ?? decimal.MaxValue);
        //                    break;

        //                case AggregationType.Max:
        //                    aggregates[column.PropertyName] = await query.MaxAsync(x =>
        //                        (decimal?)property.GetValue(x) ?? decimal.MinValue);
        //                    break;

        //                case AggregationType.Count:
        //                    aggregates[column.PropertyName] = await query.CountAsync(x =>
        //                        property.GetValue(x) != null);
        //                    break;

        //                case AggregationType.None:
        //                default:
        //                    aggregates[column.PropertyName] = "N/A"; // No aggregation
        //                    break;
        //            }
        //        }

        //        return aggregates;
        //    }


        //protected override async Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<ProductInventory> query, IReportColumnDefinition[] columns)
        //{
        //    return new Dictionary<string, dynamic>
        //    {
        //        { "InventorySupplyAtFba", await query.SumAsync(x => x.InventorySupplyAtFba) },
        //        { "SuggestedInventoryLevel", await query.SumAsync(x => x.SuggestedInventoryLevel) },
        //    };
        //}

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
