using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

public class CityPopulationFilterProvider : IFilterProvider<CityPopulation> 
{
    public Dictionary<string, Expression<Func<CityPopulation, bool>>> Filters { get; } = new()
    {
        { "Canada", p => p.CountryOrArea == "Canada" },
        { "Mexico", p => p.CountryOrArea == "Mexico" },
        { "Male", p => p.Sex == "Male" },
        { "Female", p => p.Sex == "Female" }
    };

    public IEnumerable<IEnumerable<string>> GetFacetKeys()
    {
        return new List<IEnumerable<string>>
        {
            new List<string> { "Canada", "Mexico" },
            new List<string> { "Male", "Female" }
        };
    }

    public Expression<Func<CityPopulation, bool>> GetFilter(string key)
    {
        return Filters[key];
    }
}
