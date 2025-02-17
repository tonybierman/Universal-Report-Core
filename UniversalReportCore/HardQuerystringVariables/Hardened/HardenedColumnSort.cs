using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedColumnSort : HardenedVariable<string>
    {
        public HardenedColumnSort(string sort) : base(sort) { }

        public bool Validate(List<IReportColumnDefinition> reportColumns)
        {
            // Validate querystring variables
            var baseSortKey = SortHelper.BaseSortKey(Value);

            // Find the corresponding column definition that matches the extracted sort key.
            var column = reportColumns.FirstOrDefault(c => c.PropertyName == baseSortKey);

            IsValid = column != null;

            return IsValid;
        }
    }
}
