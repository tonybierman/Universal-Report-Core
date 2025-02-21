using System.Linq;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    /// <summary>
    /// Factory class responsible for retrieving page metadata from registered providers.
    /// </summary>
    public class PageMetaFactory : IPageMetaFactory
    {
        private readonly IEnumerable<IPageMetaProvider> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageMetaFactory"/> class.
        /// </summary>
        /// <param name="providers">A collection of <see cref="IPageMetaProvider"/> instances.</param>
        public PageMetaFactory(IEnumerable<IPageMetaProvider> providers)
        {
            _providers = providers;
        }

        /// <summary>
        /// Retrieves the metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <returns>A <see cref="PageMetaViewModel"/> containing metadata for the page.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        public PageMetaViewModel GetPageMeta(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.GetPageMeta();
        }

        /// <summary>
        /// Retrieves chart metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <returns>A <see cref="ChartMetaViewModel"/> containing chart metadata for the page, or null if not available.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        public ChartMetaViewModel? GetChartMeta(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.GetChartMeta();
        }

        /// <summary>
        /// Gets the registered page metadata providers.
        /// </summary>
        public IEnumerable<IPageMetaProvider> Providers => _providers;
    }
}
