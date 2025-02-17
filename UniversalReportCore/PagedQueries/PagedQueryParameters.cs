using System.Linq;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    public class PagedQueryParameters<T> where T : class
    {
        public string? Sku { get; set; }
        public int? PageIndex { get; set; }
        public string? Sort { get; set; }
        public DateRangeFilter? DateFilter { get; set; }
        public int? ItemsPerPage { get; }
        public int[]? CohortIds { get; set; }

        public IReportColumnDefinition[] ReportColumns { get; set; }

        public Func<IQueryable<T>, IQueryable<T>>? AdditionalFilter { get; set; }
        public Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? AggregateLogic { get; set; }
        public Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? MetaLogic { get; set; }
        public Func<IQueryable<T>, int[], IQueryable<T>>? CohortLogic { get; set; }

        public PagedQueryParameters(
            IReportColumnDefinition[] columns,
            int? pageIndex,
            string? sort,
            int? itemsPerPage,
            int[]? cohortIds,
            Func<IQueryable<T>, IQueryable<T>>? additionalFilter = null,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? aggregateLogic = null,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? metaLogic = null)
        {
            ReportColumns = columns;
            PageIndex = pageIndex;
            Sort = sort;
            ItemsPerPage = itemsPerPage;
            CohortIds = cohortIds;
            AdditionalFilter = additionalFilter;
            AggregateLogic = aggregateLogic;
            MetaLogic = metaLogic;
        }
    }
}
