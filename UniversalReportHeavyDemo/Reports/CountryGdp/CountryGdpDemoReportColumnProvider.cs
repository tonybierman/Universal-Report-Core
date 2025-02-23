using UniversalReportCore;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
{
    public class CountryGdpDemoReportColumnProvider : IReportColumnProvider
    {
        public string Slug => "CountryGdpDemo";

        public List<IReportColumnDefinition> GetColumns()
        {
            return new List<IReportColumnDefinition>
            {
                new ReportColumnDefinition
                {
                    DisplayName = "Country",
                    PropertyName = "CountryName",
                    IsSortable = true,
                    DefaultSort = "Asc"

                },
                new ReportColumnDefinition
                {
                    DisplayName = "Year",
                    PropertyName = "Year",
                    IsSortable = true,
                },
                new ReportColumnDefinition
                {
                    DisplayName = "GDP (USD)",
                    PropertyName = "Value",
                    IsSortable = true
                }
            };
        }
    }
}
