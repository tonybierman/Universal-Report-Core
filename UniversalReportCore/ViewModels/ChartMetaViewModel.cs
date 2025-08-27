namespace UniversalReportCore.ViewModels
{
    /// <summary>
    /// Represents metadata for configuring a chart display.
    /// </summary>
    public class ChartMetaViewModel
    {
        public string ChartSlug { get; set; }

        /// <summary>
        /// Gets or sets the title of the chart.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle of the chart.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the label for the chart's Y axis.
        /// Optional property.
        /// </summary>
        public string? AxisYLabel { get; set; }

        /// <summary>
        /// Gets or sets the label for the chart's X axis.
        /// Optional property.
        /// </summary>
        public string? AxisXLabel { get; set; }

        public string? DataSetLabel { get; set; }

        /// <summary>
        /// Gets or sets the type of chart to display (e.g., "bar", "line", "pie").
        /// Defaults to "bar".
        /// </summary>
        public string ChartType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether zero values should be ignored in the chart data.
        /// </summary>
        public bool IgnoreZeroes { get; set; }

        /// <summary>
        /// Gets or sets the Pareto precent to breakout individually.
        /// Optional property.
        /// </summary>
        public int? ParetoPercent { get; set; }

        /// <summary>
        /// Gets or sets the API endpoint from which the chart will fetch data.
        /// </summary>
        public string DataEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the chart represents historical data.
        /// </summary>
        public bool IsHistorical { get; set; }

        /// <summary>
        /// Gets or sets the name of the partial view to be used for rendering the chart.
        /// Optional property.
        /// </summary>
        public string? PartialName { get; set; }

        public AggregationType Aggregation { get; set; }
        public TransformationType Transformation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartMetaViewModel"/> class with default values.
        /// </summary>
        public ChartMetaViewModel()
        {
            DataSetLabel = "Units";
            ChartSlug = "Default";
            ChartType = "bar";
            Aggregation = AggregationType.None;
            Transformation = TransformationType.None;
        }
    }
}
