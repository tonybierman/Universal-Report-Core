using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore
{
    public interface IReportPageHelper<TEntity, TViewModel> : IReportPageHelperBase
        where TEntity : class
        where TViewModel : class
    {
        Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters);
#pragma warning disable CS0108
        PagedQueryParameters<TEntity> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
#pragma warning restore CS0108
        TViewModel MapDictionaryToObject(Dictionary<string, dynamic> data);

        //ChartDataPoint GetChartDataTotals(Dictionary<string, dynamic> data, string key);
    }
}
