using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    /// <summary>
    /// Defines the contract for a page metadata provider, which supplies metadata information 
    /// for reports, charts, and other views in the reporting system.
    /// </summary>
    public interface IPageMetaProvider
    {
        /// <summary>
        /// Gets the unique slug identifier for the report.
        /// This is used for routing and identifying specific reports.
        /// </summary>
        string Slug { get; }

        /// <summary>
        /// Gets the category slug that groups related reports together.
        /// </summary>
        string CategorySlug { get; }

        /// <summary>
        /// Gets the page description.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Retrieves the metadata for the report page, including title and subtitle.
        /// </summary>
        /// <returns>A <see cref="PageMetaViewModel"/> containing the metadata for the report page.</returns>
        PageMetaViewModel GetPageMeta();

        /// <summary>
        /// Retrieves metadata for the report's chart representation, if applicable.
        /// </summary>
        /// <returns>A <see cref="ChartMetaViewModel"/> containing chart metadata, or null if not applicable.</returns>
        ChartMetaViewModel? GetChartMeta();

        string? GetActionWellPartial();
    }
}
