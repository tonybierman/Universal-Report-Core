// Filename: UserFilterStage.cs
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Pipeline stage for applying user-defined filters.
    /// </summary>
    public class UserFilterStage<T> : IPipelineStage<T> where T : class
    {
        private readonly Func<IQueryable<T>, IQueryable<T>> _filterPredicate;

        public UserFilterStage(Func<IQueryable<T>, IQueryable<T>> filterPredicate)
        {
            _filterPredicate = filterPredicate;
        }

        public Task<PipelineResult<T>> ExecuteAsync(PipelineResult<T> input)
        {
            var filteredQuery = _filterPredicate(input.Query);
            return Task.FromResult(new PipelineResult<T>(filteredQuery, input.Aggregates, input.Metadata));
        }
    }
}