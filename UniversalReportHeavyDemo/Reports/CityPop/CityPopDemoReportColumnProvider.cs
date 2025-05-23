﻿using UniversalReportCore;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopDemoReportColumnProvider : IReportColumnProvider
    {
        public string Slug => "CityPopDemo";

        public List<IReportColumnDefinition> GetColumns()
        {
            return new List<IReportColumnDefinition>
            {
                new ReportColumnDefinition
                {
                    IsDisplayKey = true,
                    DisplayName = "City",
                    PropertyName = "City",
                    IsSortable = true,
                    DefaultSort = "Asc"

                },
                new ReportColumnDefinition
                {
                    DisplayName = "City Type",
                    PropertyName = "CityType",
                    IsSortable = true,
                },
                new ReportColumnDefinition
                {
                    DisplayName = "Country or Area",
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
                    DisplayName = "Year",
                    PropertyName = "Year",
                    IsSortable = true
                },
                new ReportColumnDefinition
                {
                    DisplayName = "Population",
                    PropertyName = "Value",
                    IsSortable = true,
                    Aggregation = AggregationType.Sum
                }
            };
        }
    }
}
