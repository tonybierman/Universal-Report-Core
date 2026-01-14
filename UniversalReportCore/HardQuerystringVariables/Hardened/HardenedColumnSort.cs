using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for column sorting with validation logic.
    /// </summary>
    public class HardenedColumnSort : HardenedVariable<string>
    {
        /// <summary>
        /// Gets a default "null object" instance representing no sorting.
        /// </summary>
        public static HardenedColumnSort Default { get; } = new HardenedColumnSort(string.Empty);

        public HardenedColumnSort() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedColumnSort"/> class.
        /// </summary>
        /// <param name="sort">The sorting column name.</param>
        public HardenedColumnSort(string sort) : base(sort ?? string.Empty) 
        { 
            CheckSanity();
        }

        /// <summary>
        /// Checks whether the column sort is sane (non-null string).
        /// </summary>
        /// <returns>True if the value is sane; otherwise, false.</returns>
        public override bool CheckSanity()
        {
            IsSane = Value != null;
            return IsSane;
        }

        /// <summary>
        /// Validates the column sort against a list of report column definitions.
        /// </summary>
        /// <param name="reportColumns">List of report columns.</param>
        /// <returns>True if the sort column exists in the report columns; otherwise, false.</returns>
        public bool Validate(List<IReportColumnDefinition> reportColumns)
        {
            if (string.IsNullOrEmpty(Value))
            {
                IsValid = true; // Empty sort is always valid
                return true;
            }

            var baseSortKey = SortHelper.BaseSortKey(Value);
            var column = reportColumns.FirstOrDefault(c => c.PropertyName == baseSortKey);
            IsValid = column != null;
            return IsValid;
        }
    }
}
