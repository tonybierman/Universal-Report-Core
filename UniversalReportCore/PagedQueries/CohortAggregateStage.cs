// Filename: CohortAggregateStage.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Pipeline stage for computing aggregates, including cohort-based aggregations.
    /// </summary>
    public class CohortAggregateStage<T> : IPipelineStage<T> where T : class
    {
        private readonly IReportColumnDefinition[] _columns;
        private readonly int[]? _cohortIds;
        private readonly Func<IQueryable<T>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>> _computeAggregates;
        private readonly Func<IQueryable<T>, int, IQueryable<T>> _ensureCohortPredicate;
        private readonly Func<IQueryable<T>, int[]?, IQueryable<T>> _ensureAggregatePredicate;

        public CohortAggregateStage(
            IReportColumnDefinition[] columns,
            int[]? cohortIds,
            Func<IQueryable<T>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>> computeAggregates,
            Func<IQueryable<T>, int, IQueryable<T>> ensureCohortPredicate,
            Func<IQueryable<T>, int[]?, IQueryable<T>> ensureAggregatePredicate)
        {
            _columns = columns;
            _cohortIds = cohortIds;
            _computeAggregates = computeAggregates;
            _ensureCohortPredicate = ensureCohortPredicate;
            _ensureAggregatePredicate = ensureAggregatePredicate;
        }

        public async Task<PipelineResult<T>> ExecuteAsync(PipelineResult<T> input)
        {
            var aggregates = new Dictionary<string, dynamic>();

            if (_cohortIds == null || _cohortIds.Length == 0)
            {
                aggregates = await _computeAggregates(input.Query, _columns);
            }
            else
            {
                var totalAggregates = await _computeAggregates(_ensureAggregatePredicate(input.Query, _cohortIds), _columns);
                foreach (var key in totalAggregates.Keys)
                {
                    aggregates[$"{key}"] = totalAggregates[key];
                }

                foreach (var cohortId in _cohortIds)
                {
                    var cohortQuery = _ensureCohortPredicate(input.Query, cohortId);
                    var cohortResults = await _computeAggregates(cohortQuery, _columns);
                    foreach (var key in cohortResults.Keys)
                    {
                        aggregates[$"{key}_{cohortId}"] = cohortResults[key];
                    }
                }
            }

            return new PipelineResult<T>(input.Query, aggregates, input.Metadata);
        }
    }
}