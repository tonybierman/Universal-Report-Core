namespace UniversalReportCore.PagedQueries
{
    public class DateRangeFilter
    {
        public DateRangeFilter(DateTime start, DateTime end, string name)
        {
            StartDate = start;
            EndDate = end;
            PropertyName = name;
        }

        /// <summary>
        /// The start date of the filter.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the filter.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The name of the property on which the filter will be applied.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Indicates whether the date range filter has valid values for filtering.
        /// Returns true if either StartDate or EndDate is set and ColumnName is not null or empty.
        /// </summary>
        public bool HasValue => (StartDate != DateTime.MinValue || EndDate != DateTime.MinValue) && !string.IsNullOrWhiteSpace(PropertyName);
    }
}
