using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Ui
{
    public interface IReportPageHelper<TEntity, TViewModel> : IReportPageHelperBase
        where TEntity : class
        where TViewModel : class
    {
        Task<PaginatedList<TViewModel>> GetPagedDataAsync(PagedQueryParameters<TEntity> parameters, int totalCount = 0);
#pragma warning disable CS0108
        PagedQueryParameters<TEntity> CreateQueryParameters(PreQueryArguments preQueryArgs);
#pragma warning restore CS0108
        TViewModel MapDictionaryToObject(Dictionary<string, dynamic> data);

        //ChartDataPoint GetChartDataTotals(Dictionary<string, dynamic> data, string key);
    }
}
