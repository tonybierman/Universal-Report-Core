// Filename: MetadataStage.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Pipeline stage for computing metadata.
    /// </summary>
    public class MetadataStage<T> : IPipelineStage<T> where T : class
    {
        private readonly Func<IQueryable<T>, Task<Dictionary<string, dynamic>>> _ensureMeta;

        public MetadataStage(Func<IQueryable<T>, Task<Dictionary<string, dynamic>>> ensureMeta)
        {
            _ensureMeta = ensureMeta;
        }

        public async Task<PipelineResult<T>> ExecuteAsync(PipelineResult<T> input)
        {
            var metadata = await _ensureMeta(input.Query);
            return new PipelineResult<T>(input.Query, input.SearchFilters, input.Aggregates, metadata);
        }
    }
}