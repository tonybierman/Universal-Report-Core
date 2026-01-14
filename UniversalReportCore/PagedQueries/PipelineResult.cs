// Filename: PipelineResult.cs
using System.Collections.Generic;
using System.Linq;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Represents the result of a pipeline stage, containing the query and any computed data (aggregates, metadata).
    /// </summary>
    public class PipelineResult<T>
    {
        public IQueryable<T> Query { get; set; }
        public Dictionary<string, dynamic> Aggregates { get; }
        public Dictionary<string, dynamic> Metadata { get; }
        public TextFilter[]? SearchFilters { get; set; }

        public PipelineResult(IQueryable<T> query, TextFilter[]? searchFilters = null, Dictionary<string, dynamic>? aggregates = null, Dictionary<string, dynamic>? metadata = null)
        {
            Query = query;
            Aggregates = aggregates ?? new Dictionary<string, dynamic>();
            Metadata = metadata ?? new Dictionary<string, dynamic>();
            SearchFilters = searchFilters;
        }
    }
}