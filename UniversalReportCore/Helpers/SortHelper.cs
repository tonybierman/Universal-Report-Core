namespace UniversalReportCore.Helpers
{
    /// <summary>
    /// Provides helper methods for sorting report columns.
    /// </summary>
    public static class SortHelper
    {
        /// <summary>
        /// Configures the sorting state for a list of report columns based on the current sort order.
        /// </summary>
        /// <param name="columns">The list of columns to configure.</param>
        /// <param name="currentSort">The current sort order string.</param>
        public static void ConfigureSort(List<IReportColumnDefinition> columns, string currentSort)
        {
            foreach (var column in columns)
            {
                if (column.IsSortable && column.PropertyName != null)
                {
                    var isCurrentColumn = currentSort.StartsWith(column.PropertyName, StringComparison.OrdinalIgnoreCase);
                    column.IsSortDescending = isCurrentColumn && currentSort.EndsWith("Desc", StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        /// <summary>
        /// Extracts the base sort key by removing "Asc" or "Desc" from the input sort order string.
        /// </summary>
        /// <param name="input">The sort order string.</param>
        /// <returns>The base sort key without sort direction.</returns>
        public static string? BaseSortKey(string? input) => input?.Replace("Desc", "").Replace("Asc", "");

        /// <summary>
        /// Determines whether the input sort order is descending.
        /// </summary>
        /// <param name="input">The sort order string.</param>
        /// <returns><c>true</c> if the sort order is descending; otherwise, <c>false</c>.</returns>
        public static bool IsDescending(string? input) => input?.EndsWith("Desc", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Generates a human-readable description of the sort order.
        /// </summary>
        /// <param name="input">The sort order string.</param>
        /// <returns>A string describing the sort order.</returns>
        public static string? Description(string? input) =>
            input == null ? null : $"by {StringHelper.SplitPascalCase(BaseSortKey(input))} {(IsDescending(input) ? "Descending" : "Ascending")}";
    }
}
