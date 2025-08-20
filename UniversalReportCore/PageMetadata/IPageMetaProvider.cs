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
        /// Gets the literal part of the route e.g. /Reports/Index.
        /// This is used for routing.
        /// </summary>
        string RouteLiteral { get; }

        /// <summary>
        /// Gets the unique slug identifier for the report.
        /// This is used for routing and identifying specific reports.
        /// </summary>
        string Slug { get; }

        /// <summary>
        /// Gets the category slug that groups related reports together logically.
        /// </summary>
        string CategorySlug { get; }

        /// <summary>
        /// Gets the category slug that groups related reports together taxonomically.
        /// </summary>
        string TaxonomySlug { get; }

        /// <summary>
        /// Gets the page description.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Gets whether the page should be published in the report hub.
        /// </summary>
        bool IsPublished { get; }

        /// <summary>
        /// Retrieves the metadata for the report page, including title and subtitle.
        /// </summary>
        /// <returns>A <see cref="PageMetaViewModel"/> containing the metadata for the report page.</returns>
        PageMetaViewModel GetPageMeta();

        Dictionary<string, ChartMetaViewModel>? ChartMeta { get; }

        string? GetActionWellPartial();
    }
}
