using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCoreTests.Data;

namespace UniversalReportCore.Tests.Reports.Acme
{
    public class PagedAcmeQueryParameters : PagedQueryParameters<Widget>
    {
        public PagedAcmeQueryParameters(IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? itemsPerPage, int[]? cohortIds, FilterConfig<Widget> filterConfig,
            Func<IQueryable<Widget>, IQueryable<Widget>>? additionalFilter = null,
            Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>? aggregateLogic = null,
            Func<IQueryable<Widget>, Task<Dictionary<string, dynamic>>>? metaLogic = null) :
            base(columns, pageIndex, sort, itemsPerPage, cohortIds, filterConfig, additionalFilter, aggregateLogic, metaLogic)
        {
        }
    }
}
