using Microsoft.EntityFrameworkCore;
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

        protected abstract Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<T> query);
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
        public PagedQueryParameters<T> GetQuery(int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return new PagedQueryParameters<T>(
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
                        return await ComputeAggregates(filteredQuery);
                    }

                    var totalAggregates = await ComputeAggregates(EnsureAggregateQuery(filteredQuery, cohortIds));

                    foreach (var key in totalAggregates.Keys)
                    {
                        aggregates[$"{key}"] = totalAggregates[key];
                    }

                    foreach (var cohortId in cohortIds)
                    {
                        var cohortQuery = EnsureCohortQuery(filteredQuery, cohortId);
                        var cohortResults = await ComputeAggregates(cohortQuery);
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
