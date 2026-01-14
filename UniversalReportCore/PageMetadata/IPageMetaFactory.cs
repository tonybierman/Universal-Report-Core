using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    /// <summary>
    /// Defines a factory interface for retrieving page and chart metadata.
    /// </summary>
    public interface IPageMetaFactory
    {
        /// <summary>
        /// Gets the collection of registered page metadata providers.
        /// </summary>
        IEnumerable<IPageMetaProvider> Providers { get; }

        /// <summary>
        /// Retrieves the metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <param name="policy">The out string of the PageMeta Policy applied.</param>
        /// <returns>A <see cref="PageMetaViewModel"/> containing metadata for the page.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        PageMetaViewModel GetPageMeta(string slug, out string policy);

        /// <summary>
        /// Retrieves the metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <returns>A <see cref="PageMetaViewModel"/> containing metadata for the page.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        PageMetaViewModel GetPageMeta(string slug);

        /// <summary>
        /// Retrieves the chart metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public Dictionary<string, ChartMetaViewModel>? GetChartMeta(string slug);

        /// <summary>
        /// Retrieves the action well partial name for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <param name="subPartials">The list of sub-partial view models.</param>
        /// <returns>An <see cref="ActionWellViewModel"/> containing the action well configuration.</returns>
        ActionWellViewModel GetActionWell(string slug, List<SubPartialViewModel> subPartials);
    }
}
