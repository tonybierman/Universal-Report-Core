using UniversalReportCore.PageMetadata;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for a report slug with validation logic.
    /// </summary>
    public class HardenedReportSlug : HardenedVariable<string>
    {
        /// <summary>
        /// Gets the report type associated with the slug.
        /// </summary>
        public string? ReportType { get; private set; }

        public HardenedReportSlug() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedReportSlug"/> class.
        /// </summary>
        /// <param name="slug">The report slug value.</param>
        public HardenedReportSlug(string slug) : base(slug) { }

        /// <summary>
        /// Validates the slug against a page metadata factory to determine if it corresponds to a valid report type.
        /// </summary>
        /// <param name="pageMetaFactory">The page metadata factory containing available report types.</param>
        /// <returns>True if the slug corresponds to a known report type; otherwise, false.</returns>
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
