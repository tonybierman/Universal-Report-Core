using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Base class for paged query providers. Implements aggregation, filtering, and metadata retrieval for paginated data.
    /// </summary>
    /// <typeparam name="T">The entity type being queried.</typeparam>
    public abstract class BasePagedQueryProvider<T> : IPagedQueryProvider<T> where T : class
    {
        /// <summary>
        /// The unique slug identifier for this query provider.
        /// </summary>
        public abstract string Slug { get; }

        /// <summary>
        /// Default constructor for the query provider.
        /// </summary>
        public BasePagedQueryProvider() { }

        /// <summary>
        /// Computes aggregate values (Sum, Average, Min, Max, Count) for the specified columns.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <param name="columns">Columns on which aggregation is performed.</param>
        /// <returns>A dictionary containing aggregated values.</returns>
        public virtual async Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<T> query, IReportColumnDefinition[] columns)
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

        /// <summary>
        /// Retrieves metadata associated with the query.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <returns>A dictionary containing metadata.</returns>
        protected abstract Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<T> query);

        /// <summary>
        /// Applies filters to the query. Default implementation returns the original query.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <returns>The filtered query.</returns>
        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query)
        {
            return query;
        }

        /// <summary>
        /// Modifies the query to include aggregate computations based on cohort identifiers.
        /// Default implementation returns the unmodified query.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <param name="cohortIds">Array of cohort IDs.</param>
        /// <returns>The modified query.</returns>
        public virtual IQueryable<T> EnsureAggregateQuery(IQueryable<T> query, int[]? cohortIds)
        {
            return query;
        }

        /// <summary>
        /// Modifies the query to filter results by a specific cohort ID.
        /// Default implementation returns the unmodified query.
        /// </summary>
        /// <param name="query">The source query.</param>
        /// <param name="cohortId">Cohort ID to filter by.</param>
        /// <returns>The filtered query.</returns>
        public virtual IQueryable<T> EnsureCohortQuery(IQueryable<T> query, int cohortId)
        {
            return query;
        }



        /// <summary>
        /// Generates a paged query parameter object with applied filtering, aggregation, and metadata retrieval.
        /// </summary>
        /// <param name="columns">Columns to include in the query.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <param name="sort">Sorting parameter.</param>
        /// <param name="ipp">Items per page.</param>
        /// <param name="cohortIds">Array of cohort IDs.</param>
        /// <returns>A <see cref="PagedQueryParameters{T}"/> object containing query settings.</returns>
        public virtual PagedQueryParameters<T> GetQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return new PagedQueryParameters<T>(
                columns,
                pageIndex,
                sort,
                ipp,
                cohortIds,
                query => ApplyFilters((IQueryable<T>)query),
                src => ComputeAggregatesWithCohortsAsync(src, columns, cohortIds),
                src => ComputeMetaAsync(src)
            );
        }

        private async Task<Dictionary<string, dynamic>> ComputeAggregatesWithCohortsAsync(IQueryable<T> src, IReportColumnDefinition[] columns, int[]? cohortIds)
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
        }

        private async Task<Dictionary<string, dynamic>> ComputeMetaAsync(IQueryable<T> src)
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

    }
}
