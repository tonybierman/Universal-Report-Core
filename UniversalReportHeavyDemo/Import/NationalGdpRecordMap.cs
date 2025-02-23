using CsvHelper.Configuration;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Import
{
    public class NationalGdpRecordMap : ClassMap<NationalGdp>
    {
        public NationalGdpRecordMap()
        {
            Map(m => m.CountryName).Name("CountryName");
            Map(m => m.CountryCode).Name("CountryCode");
            Map(m => m.Year).Name("Year");
            Map(m => m.Value).Name("Value");
        }
    }
}
