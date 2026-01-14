using System.Text.RegularExpressions;
using UniversalReportCore.Helpers;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    /// <summary>
    /// Represents a hardened variable for search queries with validation logic.
    /// </summary>
    public class HardenedSearchQueries : HardenedVariable<Dictionary<string, string>?>
    {
        /// <summary>
        /// Gets a default "null object" instance representing no search queries.
        /// </summary>
        public static HardenedSearchQueries Default { get; } = new HardenedSearchQueries(null);

        public HardenedSearchQueries() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedSearchQueries"/> class.
        /// </summary>
        /// <param name="searchDict">The search query dictionary.</param>
        public HardenedSearchQueries(Dictionary<string, string>? searchDict)
        {
            Value = searchDict;
            CheckSanity();
        }

        /// <summary>
        /// Checks whether the search queries are sane.
        /// </summary>
        /// <returns>True if the value is sane; otherwise, false.</returns>
        public override bool CheckSanity()
        {
            IsSane = true; // Dictionary can be null or empty, both are sane
            return IsSane;
        }

        /// <summary>
        /// Validates the search queries against report column definitions.
        /// </summary>
        /// <param name="reportColumns">List of report columns.</param>
        /// <returns>True if all search queries are valid; otherwise, false.</returns>
        public bool Validate(List<IReportColumnDefinition> reportColumns)
        {
            if (Value == null || Value.Count == 0)
            {
                IsValid = true;
                return true;
            }

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
