namespace UniversalReportCore.PagedQueries
{
    /// <summary>
    /// Represents a date range filter that can be applied to a dataset.
    /// </summary>
    public class DateRangeFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeFilter"/> class.
        /// </summary>
        /// <param name="start">The start date of the filter range.</param>
        /// <param name="end">The end date of the filter range.</param>
        /// <param name="name">The name of the property to which the filter applies.</param>
        public DateRangeFilter(DateTime start, DateTime end, string name)
        {
            StartDate = start;
            EndDate = end;
            PropertyName = name;
        }

        /// <summary>
        /// Gets or sets the start date of the filter range.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the filter range.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the property on which the filter will be applied.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Determines whether the filter has a valid date range for filtering.
        /// </summary>
        /// <remarks>
        /// The filter is considered valid if:
        /// - Either <see cref="StartDate"/> or <see cref="EndDate"/> is set (i.e., not <see cref="DateTime.MinValue"/>).
        /// - The <see cref="PropertyName"/> is not null, empty, or whitespace.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if the filter has a valid date range; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValue => (StartDate != DateTime.MinValue || EndDate != DateTime.MinValue) && !string.IsNullOrWhiteSpace(PropertyName);
    }
}
