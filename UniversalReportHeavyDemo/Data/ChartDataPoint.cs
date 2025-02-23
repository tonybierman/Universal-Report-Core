namespace UniversalReportHeavyDemo.Data
{
    public class ChartDataPoint
    {
        public string Label { get; set; }  // e.g., Date, Product Name
        public Dictionary<string, double> Values { get; set; } = new(); // Supports multiple named datasets
        public string DefaultKey { get; set; } = "default"; // Configurable default key

        // Backward compatibility for single dataset with configurable default key
        public double Value
        {
            get => Values.TryGetValue(DefaultKey, out var val) ? val : 0;
            set => Values[DefaultKey] = value;
        }
    }
}
