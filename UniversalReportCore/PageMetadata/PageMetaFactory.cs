using System.Linq;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;

namespace ProductionPlanner.PageMetadata
{
    public class PageMetaFactory : IPageMetaFactory
    {
        private readonly IEnumerable<IPageMetaProvider> _providers;

        public PageMetaFactory(IEnumerable<IPageMetaProvider> providers)
        {
            _providers = providers;
        }

        public PageMetaViewModel GetPageMeta(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.GetPageMeta();
        }

        public ChartMetaViewModel? GetChartMeta(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.GetChartMeta();
        }

        public IEnumerable<IPageMetaProvider> Providers => _providers;
    }
}
