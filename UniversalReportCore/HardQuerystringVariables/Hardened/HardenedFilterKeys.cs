using AutoMapper.Configuration.Annotations;
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
        public HardenedFilterKeys(string[]? filterKeys) : base(filterKeys) { IsSane = true; }

        /// <summary>
        /// Checks if the given filter key is present in the array.
        /// </summary>
        /// <param name="filterKey">The filter key to check.</param>
        /// <returns>True if the filter key exists, false otherwise.</returns>
        public bool Contains(string filterKey)
        {
            return Value != null && Value.Contains(filterKey);
        }

        /// <summary>
        /// Validates the filter key array against a filter provider to determine if each element corresponds to a valid facet key.
        /// </summary>
        /// <param name="filter">The filter provider containing available facet keys.</param>
        /// <returns>True if the filter key array is null, if the elements are null, or if each element corresponds to a known facet key; otherwise, false.</returns>
        public bool Validate(IFilterProviderBase filter)
        {
            if (Value == null)
            {
                IsValid = true;
            }
            else
            {
                var validValues = filter.GetFacetKeys().Values.SelectMany(v => v).ToHashSet(); // Flatten all valid values
                IsValid = Value.All(k => k == null || validValues.Contains(k)); // Check if all selected values exist
            }

            return IsValid;
        }
    }
}
