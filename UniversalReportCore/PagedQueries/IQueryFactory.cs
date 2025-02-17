using UniversalReportCore.PagedQueries;

namespace ProductionPlanner.PagedQueries
{
    public interface IQueryFactory<T> where T : class
    {
        PagedQueryParameters<T> CreateQueryParameters(string queryType, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
    }
}
