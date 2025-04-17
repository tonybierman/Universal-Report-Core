// Filename: IPipelineStage.cs
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Interface for pipeline stages that process the query or produce outputs.
    /// </summary>
    public interface IPipelineStage<T>
    {
        Task<PipelineResult<T>> ExecuteAsync(PipelineResult<T> input);
    }
}