using System.Linq;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.PageMetadata;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for cohort identifiers with sanity and validation checks.
    /// </summary>
    public class HardenedFilterKeys : HardenedVariable<string[]?>
    {
        public HardenedFilterKeys() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedCohortIdentifiers"/> class.
        /// </summary>
        /// <param name="filterKeys">The array of cohort IDs.</param>
        public HardenedFilterKeys(string[]? filterKeys) : base(filterKeys) { IsValid = true; IsSane = true; }
    }
}
