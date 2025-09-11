using System.Text.RegularExpressions;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedSearchQueries : HardenedVariable<Dictionary<string, string>?>
    {
        public HardenedSearchQueries() { }

        public HardenedSearchQueries(Dictionary<string, string> searchDict)
        {
            Value = searchDict;
        }

        public bool Validate(List<IReportColumnDefinition> reportColumns)
        {
            if (Value == null) return true;

            foreach (var pair in Value)
            {
                if (!IsSafeQueryString(pair.Key) || !IsSafeQueryString(pair.Value) ||
                    !reportColumns.Any(c => c.IsSearchable && c.PropertyName == pair.Key))
                {
                    IsValid = false;
                    return false;
                }
            }
            IsValid = true;
            return true;
        }
    }
}