// Filename: QueryPipeline.cs
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Manages the execution of a sequence of pipeline stages.
    /// </summary>
    public class QueryPipeline<T> where T : class
    {
        private readonly List<IPipelineStage<T>> _stages = new();

        public QueryPipeline<T> AddStage(IPipelineStage<T> stage)
        {
            _stages.Add(stage);
            return this;
        }

        public async Task<PagedQueryParameters<T>> ExecuteAsync(
            IQueryable<T> query,
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? ipp,
            int[]? cohortIds)
        {
            var result = new PipelineResult<T>(query);
            foreach (var stage in _stages)
            {
                result = await stage.ExecuteAsync(result);
            }

            return new PagedQueryParameters<T>(
                columns,
                pageIndex,
                sort,
                ipp,
                cohortIds,
                _ => result.Query,
                _ => Task.FromResult(result.Aggregates),
                _ => Task.FromResult(result.Metadata)
            );
        }
    }
}