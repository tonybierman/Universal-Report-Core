using System.Linq;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for cohort identifiers with sanity and validation checks.
    /// </summary>
    public class HardenedCohortIdentifiers : HardenedVariable<int[]?>
    {
        private readonly int _min = 1;
        private readonly int _max = 10000;
        private readonly int maxArraySize = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedCohortIdentifiers"/> class.
        /// </summary>
        /// <param name="cohortIds">The array of cohort IDs.</param>
        public HardenedCohortIdentifiers(int[]? cohortIds) : base(cohortIds) { }

        /// <summary>
        /// Checks whether the cohort identifiers are within valid limits.
        /// </summary>
        /// <returns>True if the values are sane; otherwise, false.</returns>
        public override bool CheckSanity()
        {
            if (Value == null || Value.Length == 0)
            {
                IsSane = true; // Null or empty arrays are considered sane.
            }
            else if (Value.Length > maxArraySize || Value.Any(id => id < _min || id > _max))
            {
                IsSane = false; // Ensure array size and ID ranges are within valid limits.
            }
            else if (Value.Distinct().Count() != Value.Length)
            {
                IsSane = false; // Ensure there are no duplicate IDs.
            }
            else
            {
                IsSane = true;
            }

            return IsSane;
        }

        /// <summary>
        /// Validates the cohort IDs against a provided list of valid cohorts.
        /// </summary>
        /// <param name="cohorts">An optional list of valid cohorts.</param>
        /// <returns>True if all cohort IDs exist in the valid set; otherwise, false.</returns>
        public bool Validate(ICohort[]? cohorts = null)
        {
            if (Value == null || Value.Length == 0)
            {
                IsValid = true; // No cohort IDs to validate.
            }
            else
            {
                var validCohortIds = (cohorts ?? Array.Empty<Cohort>()).Select(c => c.Id).ToHashSet();
                IsValid = Value.All(id => validCohortIds.Contains(id)); // Check if all IDs exist in the valid set.
            }

            return IsValid;
        }
    }
}
