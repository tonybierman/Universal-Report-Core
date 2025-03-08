using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

public class CityPopulationFilterProvider : IFilterProvider<CityPopulation> 
{
    public Dictionary<string, Expression<Func<CityPopulation, bool>>> Filters { get; } = new()
    {
        { "Canada", p => p.CountryOrArea == "Canada" },
        { "Mexico", p => p.CountryOrArea == "Mexico" },
        { "Pakistan", p => p.CountryOrArea == "Pakistan" },
        { "Japan", p => p.CountryOrArea == "Japan" },
        { "Male", p => p.Sex == "Male" },
        { "Female", p => p.Sex == "Female" }
    };

    public Dictionary<string, List<string>> GetFacetKeys()
    {
        return new Dictionary<string, List<string>>
        {
            { "Country", new List<string> { "Canada", "Mexico", "Pakistan", "Japan" } },
            { "Gender", new List<string> { "Male", "Female" } }
        };
    }

    public Expression<Func<CityPopulation, bool>> GetFilter(string key)
    {
        return Filters[key];
    }
}
