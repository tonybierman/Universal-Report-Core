using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    /// <summary>
    /// Provides a base implementation for page metadata providers.
    /// This base class can be extended by specific page metadata providers to define additional metadata.
    /// </summary>
    public class BaseReportPageMetaProvider : BasePageMetaProvider
    {
        public override string? RouteLiteral => "/Reports/Index";
        public override string CategorySlug => "Reports";
        public override string? TaxonomySlug => CategorySlug;
        public override string? Description => null;

        /// <summary>
        /// Retrieves metadata for the report's chart representation.
        /// This method can be overridden by derived classes to provide custom chart metadata.
        /// </summary>
        /// <returns>
        /// A <see cref="ChartMetaViewModel"/> containing the chart metadata, or <c>null</c> if no chart metadata is available.
        /// </returns>
        public virtual ChartMetaViewModel? GetChartMeta() => null;

        public virtual string? GetActionWellPartial() => null;
    }
}
