using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    public interface IQueryFactory<T> where T : class
    {
        PagedQueryParameters<T> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
    }
}
