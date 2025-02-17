using System.Linq;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedCohortIdentifiers : HardenedVariable<int[]?>
    {
        private readonly int _min = 1;
        private readonly int _max = 10000;
        private readonly int maxArraySize = 100;

        public HardenedCohortIdentifiers(int[]? cohortIds) : base(cohortIds) { }

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

        public bool Validate(Cohort[]? cohorts = null)
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
