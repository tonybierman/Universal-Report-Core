using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for column sorting with validation logic.
    /// </summary>
    public class HardenedColumnSort : HardenedVariable<string>
    {
        public HardenedColumnSort() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedColumnSort"/> class.
        /// </summary>
        /// <param name="sort">The sorting column name.</param>
        public HardenedColumnSort(string sort) : base(sort) { }

        /// <summary>
        /// Validates the column sort against a list of report column definitions.
        /// </summary>
        /// <param name="reportColumns">List of report columns.</param>
        /// <returns>True if the sort column exists in the report columns; otherwise, false.</returns>
        public bool Validate(List<IReportColumnDefinition> reportColumns)
        {
            var baseSortKey = SortHelper.BaseSortKey(Value);
            var column = reportColumns.FirstOrDefault(c => c.PropertyName == baseSortKey);
            IsValid = column != null;
            return IsValid;
        }
    }
}
