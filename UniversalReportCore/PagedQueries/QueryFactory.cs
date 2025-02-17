using UniversalReportCore.PagedQueries;

namespace ProductionPlanner.PagedQueries
{
    public class QueryFactory<T> : IQueryFactory<T> where T : class
    {
        private readonly IEnumerable<IPagedQueryProvider<T>> _providers;

        public QueryFactory(IEnumerable<IPagedQueryProvider<T>> providers)
        {
            _providers = providers;
        }

        public PagedQueryParameters<T> CreateQueryParameters(string slug, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported report type: {slug}");
            }

            return provider.GetQuery(pageIndex, sort, ipp, cohortIds);
        }
    }

}

