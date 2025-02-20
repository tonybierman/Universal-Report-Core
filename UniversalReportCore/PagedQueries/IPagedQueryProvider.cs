using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    public interface IPagedQueryProvider<T> where T : class
    {
        string Slug { get; }
        PagedQueryParameters<T> GetQuery(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
        IQueryable<T> EnsureAggregateQuery(IQueryable<T> query, int[]? cohortIds);
        IQueryable<T> EnsureCohortQuery(IQueryable<T> query, int cohortId);
    }
}
