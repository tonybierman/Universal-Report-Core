using ProductionPlanner.PageMetadata;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.PageMetadata;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedReportSlug : HardenedVariable<string>
    {
        public string? ReportType { get; private set; }

        public HardenedReportSlug(string slug) : base(slug) { }

        public bool Validate(IPageMetaFactory pageMetaFactory)
        {
            ReportType = pageMetaFactory.Providers
                .Where(p => p.Slug == Value)
                .Select(p => p.CategorySlug)
                .FirstOrDefault();

            IsValid = !string.IsNullOrWhiteSpace(ReportType);

            return IsValid;
        }
    }
}
