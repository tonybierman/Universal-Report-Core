using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    /// <summary>
    /// Provides a base implementation for page metadata providers.
    /// This base class can be extended by specific page metadata providers to define additional metadata.
    /// </summary>
    public class BasePageMetaProvider
    {
        public virtual bool IsPublished => true;
        public virtual string? RouteLiteral => null;
        public virtual string? TaxonomySlug => null;
        public virtual string? CategorySlug => null;
        public virtual string? Description => null;

        public ActionWellViewModel GetActionWell(IList<SubPartialViewModel> subPartials)
        {
            return new ActionWellViewModel(subPartials);
        }
    }
}
