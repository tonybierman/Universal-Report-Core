using System.Security.Policy;

namespace UniversalReportCore.ViewModels
{
    public class ChartMetaViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string? AxisLabel { get; set; }
        public string ChartType { get; set; }
        public bool IgnoreZeroes { get; set; }
        public int? TakeTop { get; set; }
        public string DataEndpoint { get; set; }
        public bool IsHistorical { get; set; }

        public ChartMetaViewModel() 
        {
            ChartType = "bar";
        }
    }
}
