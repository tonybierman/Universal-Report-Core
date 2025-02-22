using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore
{
    public interface IReportPageHelper<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        string DefaultSort { get; }
        Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters);
        List<IReportColumnDefinition> GetReportColumns(string slug);
        PagedQueryParameters<TEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
        Task<ICohort[]?> GetCohortsAsync(int[] cohortIds);
        TViewModel MapDictionaryToObject(Dictionary<string, dynamic> data);

        //ChartDataPoint GetChartDataTotals(Dictionary<string, dynamic> data, string key);
    }
}
