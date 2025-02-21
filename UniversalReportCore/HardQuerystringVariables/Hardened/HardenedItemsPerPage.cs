using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for items per page with sanity checks.
    /// </summary>
    public class HardenedItemsPerPage : HardenedVariable<int?>
    {
        private readonly int _min = 0;
        private readonly int _max = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedItemsPerPage"/> class.
        /// </summary>
        /// <param name="ipp">The number of items per page.</param>
        public HardenedItemsPerPage(int? ipp) : base(ipp) { }

        /// <summary>
        /// Checks whether the number of items per page is within a valid range.
        /// </summary>
        /// <returns>True if the value is sane; otherwise, false.</returns>
        public override bool CheckSanity()
        {
            if (Value == null)
            {
                IsSane = true; // Null values are considered valid.
            }
            else
            {
                IsSane = Value >= _min && Value <= _max;
            }

            IsValid = IsSane;
            return IsSane;
        }
    }
}
