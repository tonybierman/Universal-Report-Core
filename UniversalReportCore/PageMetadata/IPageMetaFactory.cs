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
        /// <returns>A <see cref="PageMetaViewModel"/> containing metadata for the page.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        PageMetaViewModel GetPageMeta(string slug, out string policy);

        /// <summary>
        /// Retrieves the chart metadata for a given page based on its slug.
        /// </summary>
        /// <param name="slug">The slug identifying the page.</param>
        /// <returns>A <see cref="ChartMetaViewModel"/> containing chart metadata for the page, or null if not available.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no provider is found for the specified slug.</exception>
        ChartMetaViewModel? GetChartMeta(string slug);

        string? GetActionWellPartial(string slug);
    }
}
