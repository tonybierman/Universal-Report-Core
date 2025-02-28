namespace UniversalReportCore.Data
{
    /// <summary>
    /// Represents a single data point for charting, supporting multiple named datasets.
    /// </summary>
    public class ChartDataPoint
    {
        /// <summary>
        /// Gets or sets the label for the data point (e.g., date, product name).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the dictionary containing values for different datasets.
        /// The keys represent dataset names, and the values represent numerical data points.
        /// </summary>
        public Dictionary<string, double> Values { get; set; } = new();

        /// <summary>
        /// Gets or sets the default key used for retrieving and setting the primary value.
        /// Default is "default".
        /// </summary>
        public string DefaultKey { get; set; } = "default";

        /// <summary>
        /// Gets or sets the value associated with the <see cref="DefaultKey"/>.
        /// If the key does not exist in <see cref="Values"/>, it returns 0.
        /// </summary>
        public double Value
        {
            get => Values.TryGetValue(DefaultKey, out var val) ? val : 0;
            set => Values[DefaultKey] = value;
        }
    }
}
