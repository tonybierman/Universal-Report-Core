using CsvHelper.Configuration;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Import
{
    public class CityPopulationRecordMap : ClassMap<CityPopulation>
    {
        public CityPopulationRecordMap()
        {
            Map(m => m.CountryOrArea).Name("Country");
            Map(m => m.Year).Name("Year");
            Map(m => m.Area).Name("Area");
            Map(m => m.Sex).Name("Sex");
            Map(m => m.City).Name("City");
            Map(m => m.CityType).Name("CityType");
            Map(m => m.RecordType).Name("RecordType");
            Map(m => m.Reliability).Name("Reliability");
            Map(m => m.SourceYear).Name("SourceYear");
            Map(m => m.Value).Name("Value");
            Map(m => m.ValueFootnotes).Name("ValueFootnotes");
        }
    }
}
