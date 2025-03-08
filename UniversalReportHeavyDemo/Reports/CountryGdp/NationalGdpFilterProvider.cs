using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class NationalGdpFilterProvider : IFilterProvider<NationalGdp>
    {
        public Dictionary<string, Expression<Func<NationalGdp, bool>>> Filters { get; } = new()
    {
        { "Canada", p => p.CountryName == "Canada" },
        { "Mexico", p => p.CountryName == "Mexico" },
        { "Pakistan", p => p.CountryName == "Pakistan" },
        { "Japan", p => p.CountryName == "Japan" }
    };

        public Dictionary<string, List<string>> GetFacetKeys()
        {
            return new Dictionary<string, List<string>>
            {
                { "Country", new List<string> { "Canada", "Mexico", "Pakistan", "Japan" } }
            };
        }

        public Expression<Func<NationalGdp, bool>> GetFilter(string key)
        {
            return Filters[key];
        }
    }
}
