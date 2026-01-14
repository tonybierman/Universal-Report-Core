namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Represents a date range filter that can be applied to a dataset.
    /// </summary>
    public class TextFilter
    {
        public TextFilter()
        {
        }

        public TextFilter(string value, string name)
        {

            PropertyName = name;
            Value = value;
        }

        public string Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the property on which the filter will be applied.
        /// </summary>
        public string PropertyName { get; set; } = default!;

        public bool HasValue => !string.IsNullOrWhiteSpace(Value) && !string.IsNullOrWhiteSpace(PropertyName);
    }
}
