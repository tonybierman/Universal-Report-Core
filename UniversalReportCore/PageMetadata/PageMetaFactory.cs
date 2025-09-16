using System.Linq;
using System.Reflection;
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
        /// Retrieves the metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <param name="policy">The out string of the PageMeta Policy applied.</param>
        /// <returns>A <see cref="PageMetaViewModel"/> containing metadata for the page.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        public PageMetaViewModel GetPageMeta(string slug, out string policy)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            var attribute = provider.GetType().GetCustomAttribute<PageMetaPolicyAttribute>();
            policy = attribute?.Policy; // Null if no policy (anonymous)

            return provider.GetPageMeta();
        }


        /// <summary>
        /// Retrieves the action well partial view name associated with the specified slug.
        /// </summary>
        /// <param name="slug">The unique identifier for the provider whose action well partial is to be retrieved. Cannot be null or
        /// empty.</param>
        /// <returns>The name of the action well partial view associated with the specified slug, or <see langword="null"/> if no
        /// partial view is defined.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified <paramref name="slug"/>.</exception>
        public ActionWellViewModel GetActionWell(string slug, List<SubPartialViewModel> subPartials)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.GetActionWell(subPartials);
        }

        /// <summary>
        /// Retrieves the chart metadata associated with the specified slug.
        /// </summary>
        /// <param name="slug">The unique identifier for the provider whose chart metadata is to be retrieved. Cannot be null or empty.</param>
        /// <returns>A dictionary containing chart metadata, where the keys represent chart identifiers and the values are <see
        /// cref="ChartMetaViewModel"/> objects.  Returns <see langword="null"/> if no metadata is available.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified <paramref name="slug"/>.</exception>
        public Dictionary<string, ChartMetaViewModel>? GetChartMeta(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported meta for page: {slug}");
            }

            return provider.ChartMeta;
        }

        /// <summary>
        /// Gets the registered page metadata providers.
        /// </summary>
        public IEnumerable<IPageMetaProvider> Providers => _providers;
    }
}
