namespace UniversalReportCore.Helpers
{
    public static class SortHelper
    {
        public static void ConfigureSort(List<IReportColumnDefinition> columns, string currentSort)
        {
            foreach (var column in columns)
            {
                var isCurrentColumn = currentSort.StartsWith(column.PropertyName, StringComparison.OrdinalIgnoreCase);
                column.IsSortDescending = isCurrentColumn && currentSort.EndsWith("Desc", StringComparison.OrdinalIgnoreCase);
            }
        }
        public static string? BaseSortKey(string? input) => input?.Replace("Desc", "").Replace("Asc", "");
        public static bool IsDescending(string? input) => input?.EndsWith("Desc", StringComparison.OrdinalIgnoreCase) ?? false;
        public static string? Description(string? input) =>
            input == null ? null : $"by {BaseSortKey(input)} {(IsDescending(input) ? "Descending" : "Ascending")}";
    }
}
