using UniversalReportCore;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.PagedQueries
{
    public class QueryFactory<T> : IQueryFactory<T> where T : class
    {
        private readonly IEnumerable<IPagedQueryProvider<T>> _providers;

        public QueryFactory(IEnumerable<IPagedQueryProvider<T>> providers)
        {
            _providers = providers;
        }

        public PagedQueryParameters<T> CreateQueryParameters(string slug, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported query type: {slug}");
            }

            return provider.GetQuery(columns, pageIndex, sort, ipp, cohortIds);
        }
    }

}

