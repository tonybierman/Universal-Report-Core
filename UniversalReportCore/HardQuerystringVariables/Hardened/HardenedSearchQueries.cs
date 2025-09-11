using System.Text.RegularExpressions;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedSearchQueries : HardenedVariable<Dictionary<string, string>?>
    {
        public HardenedSearchQueries() { }

        public HardenedSearchQueries(Dictionary<string, string> searchDict)
        {
            Value = searchDict;
        }

        public bool Validate()
        {
            if (Value == null)
            {
                IsValid = true;
                return IsValid;
            }

            foreach (var pair in Value)
            {
                if (!IsSafeQueryString(pair.Key) || !IsSafeQueryString(pair.Value))
                {
                    IsValid = false;
                    return IsValid;
                }
            }

            IsValid = true;
            return IsValid;
        }
    }
}