using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for the paging index with validation logic.
    /// </summary>
    public class HardenedPagingIndex : HardenedVariable<int?>
    {
        private readonly int _min = 1;
        private readonly int _max = 10000;

        public HardenedPagingIndex() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedPagingIndex"/> class.
        /// </summary>
        /// <param name="index">The paging index.</param>
        public HardenedPagingIndex(int? index) : base(index) { }

        /// <summary>
        /// Checks whether the paging index is within a valid range.
        /// </summary>
        /// <returns>True if the index is sane; otherwise, false.</returns>
        public override bool CheckSanity()
        {
            IsSane = Value == null || Value >= _min && Value <= _max;
            return IsSane;
        }

        /// <summary>
        /// Validates the paging index against the total number of available pages.
        /// </summary>
        /// <param name="totalPages">The total available pages.</param>
        /// <returns>True if the index is within the valid range; otherwise, false.</returns>
        public bool Validate(int totalPages)
        {
            // TODO: totalPages == 0 is not really valid condition here
            IsValid = Value == null || Value <= totalPages || totalPages == 0;
            return IsValid;
        }
    }
}
