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

        public virtual Dictionary<string, ChartMetaViewModel>? ChartMeta { get => null; }

    }
}
