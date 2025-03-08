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
        { "Mexico", p => p.CountryName == "Mexico" }
    };

        public IEnumerable<IEnumerable<string>> GetFacetKeys()
        {
            return new List<IEnumerable<string>>
        {
            new List<string> { "Canada", "Mexico" },
            new List<string> { "Male", "Female" }
        };
        }

        public Expression<Func<NationalGdp, bool>> GetFilter(string key)
        {
            return Filters[key];
        }
    }
}
