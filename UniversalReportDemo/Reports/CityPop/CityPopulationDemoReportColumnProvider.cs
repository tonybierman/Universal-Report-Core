using UniversalReportCore;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationDemoReportColumnProvider : IReportColumnProvider
    {
        public string Slug => "CityPopulationDemo";

        public List<IReportColumnDefinition> GetColumns()
        {
            return new List<IReportColumnDefinition>
            {
                new ReportColumnDefinition
                {
                    DisplayName = "City",
                    PropertyName = "City",
                    IsSortable = true,
                    DefaultSort = "Desc"

                },
                new ReportColumnDefinition
                {
                    DisplayName = "CityType",
                    PropertyName = "CityType",
                    IsSortable = true,
                },
                new ReportColumnDefinition
                {
                    DisplayName = "CountryOrArea",
                    PropertyName = "CountryOrArea",
                    IsSortable = true
                },
                new ReportColumnDefinition
                {
                    DisplayName = "Sex",
                    PropertyName = "Sex",
                    IsSortable = true
                },
                new ReportColumnDefinition
                {
                    DisplayName = "Value",
                    PropertyName = "Value",
                    IsSortable = true
                }
            };
        }
    }
}
