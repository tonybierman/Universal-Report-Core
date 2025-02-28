using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UniversalReport.Services
{
    public interface IUniversalReportService
    {
        Task<PaginatedList<TViewModel>> GetPagedAsync<TEntity, TViewModel>(PagedQueryParameters<TEntity> parameters, int totalCount, IQueryable<TEntity>? query = null)
            where TEntity : class
            where TViewModel : class;
    }
}
