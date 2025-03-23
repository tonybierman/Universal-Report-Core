using UniversalReportCore.PagedQueries;
using UniversalReportCoreTests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.Tests.Reports.Acme
{
    public class PagedAcmeQueryProvider : BasePagedQueryProvider<Widget>
    {
        private readonly AcmeDbContext _dbContext;

        //public PagedAcmeQueryProvider() : this(null)
        //{
        //}

        public PagedAcmeQueryProvider(AcmeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override string Slug => "acme-query-provider";

        public override PagedQueryParameters<Widget> BuildPagedQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, IQueryable<Widget>? reportQuery = null)
        {
            return new PagedQueryParameters<Widget>(
                columns,
                pageIndex,
                sort,
                ipp,
                cohortIds,
                query => EnsureUserFiltersPredicate(EnsureReportQuery() ?? query),
                async (IQueryable<Widget> src) => await ComputeAggregatesWithCohortsAsync(src, columns, cohortIds),
                async (IQueryable<Widget> src) => await ComputeMetaAsync(src)
            );
        }

        protected override async Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<Widget> query)
        {
            return new Dictionary<string, dynamic>(); // Placeholder for metadata logic
        }

        /// <summary>
        /// Computes aggregates based on cohort IDs.
        /// </summary>
        public async Task<Dictionary<string, dynamic>> ComputeAggregatesWithCohortsAsync(IQueryable<Widget> src, IReportColumnDefinition[] columns, int[]? cohortIds)
        {
            var aggregates = new Dictionary<string, dynamic>();
            var filteredQuery = EnsureUserFiltersPredicate(src);

            if (cohortIds == null || cohortIds.Length == 0)
            {
                return await ComputeAggregates(filteredQuery, columns);
            }

            var totalAggregates = await ComputeAggregates(EnsureAggregatePredicate(filteredQuery, cohortIds), columns);

            foreach (var key in totalAggregates.Keys)
            {
                aggregates[$"{key}"] = totalAggregates[key];
            }

            foreach (var cohortId in cohortIds)
            {
                var cohortQuery = EnsureCohortPredicate(filteredQuery, cohortId);
                var cohortResults = await ComputeAggregates(cohortQuery, columns);
                foreach (var key in cohortResults.Keys)
                {
                    aggregates[$"{key}_{cohortId}"] = cohortResults[key];
                }
            }

            return aggregates;
        }

        public async override Task<Dictionary<string, dynamic>> ComputeAggregates(IQueryable<Widget> query, IReportColumnDefinition[] columns)
        {
            return await base.ComputeAggregates(query, columns);
        }

        /// <summary>
        /// Computes metadata for the query.
        /// </summary>
        private async Task<Dictionary<string, dynamic>> ComputeMetaAsync(IQueryable<Widget> src)
        {
            var meta = new Dictionary<string, dynamic>();
            var filteredQuery = EnsureUserFiltersPredicate(src);

            var metaDictionary = await EnsureMeta(filteredQuery);
            foreach (var key in metaDictionary.Keys)
            {
                meta[$"{key}"] = metaDictionary[key];
            }

            return meta;
        }
    }
}
