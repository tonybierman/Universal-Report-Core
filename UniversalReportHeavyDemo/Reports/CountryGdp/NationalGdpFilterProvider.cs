using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class NationalGdpFilterProvider : BaseFilterProvider<NationalGdp>
    {
        public NationalGdpFilterProvider() : base(new List<Facet<NationalGdp>>
        {
            new("CountryOrArea", new()
            {
                new("Canada", p => p.CountryName == "Canada"),
                new("Mexico", p => p.CountryName == "Mexico"),
                new("Pakistan", p => p.CountryName == "Pakistan"),
                new("Japan", p => p.CountryName == "Japan"),
            })
        })
        { }
    }
}
