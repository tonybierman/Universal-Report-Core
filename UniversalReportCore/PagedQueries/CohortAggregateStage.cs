// Filename: CohortAggregateStage.cs
using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IFilterConfig<T>? _filterConfig;
        private readonly Func<IQueryable<T>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>> _computeAggregates;
        private readonly Func<IQueryable<T>, int, IQueryable<T>> _ensureCohortPredicate;
        private readonly Func<IQueryable<T>, int[]?, IQueryable<T>> _ensureAggregatePredicate;

        public CohortAggregateStage(
            IReportColumnDefinition[] columns,
            int[]? cohortIds,
            IFilterConfig<T>? filterConfig,
            Func<IQueryable<T>, IReportColumnDefinition[], Task<Dictionary<string, dynamic>>> computeAggregates,
            Func<IQueryable<T>, int, IQueryable<T>> ensureCohortPredicate,
            Func<IQueryable<T>, int[]?, IQueryable<T>> ensureAggregatePredicate)
        {
            _columns = columns;
            _cohortIds = cohortIds;
            _filterConfig = filterConfig;
            _computeAggregates = computeAggregates;
            _ensureCohortPredicate = ensureCohortPredicate;
            _ensureAggregatePredicate = ensureAggregatePredicate;
        }

        public async Task<PipelineResult<T>> ExecuteAsync(PipelineResult<T> input)
        {
            var aggregates = new Dictionary<string, dynamic>();

            var query1 = input.Query;
            if (_filterConfig != null && _filterConfig.FilterKeys != null && _filterConfig.FilterKeys.Any())
            {
                var predicate = _filterConfig.FilterFactory.BuildPredicate(_filterConfig.FilterKeys);
                query1 = query1.Where(predicate);
            }

            if (_cohortIds == null || _cohortIds.Length == 0)
            {
                aggregates = await _computeAggregates(query1, _columns);
            }
            else
            {
                var totalAggregates = await _computeAggregates(_ensureAggregatePredicate(query1, _cohortIds), _columns);
                foreach (var key in totalAggregates.Keys)
                {
                    aggregates[$"{key}"] = totalAggregates[key];
                }

                foreach (var cohortId in _cohortIds)
                {
                    var cohortQuery = _ensureCohortPredicate(query1, cohortId);
                    var cohortResults = await _computeAggregates(cohortQuery, _columns);
                    foreach (var key in cohortResults.Keys)
                    {
                        aggregates[$"{key}_{cohortId}"] = cohortResults[key];
                    }
                }
            }

            return new PipelineResult<T>(query1, aggregates, input.Metadata);
        }
    }
}