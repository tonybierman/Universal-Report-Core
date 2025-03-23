using System.Linq.Expressions;
using UniversalReportHeavyDemo.Data;

public interface ICityPopulationFilterProvider
{
    Dictionary<string, Expression<Func<CityPopulation, bool>>> Filters { get; }

    IEnumerable<IEnumerable<string>> GetFacetKeys();
    Expression<Func<CityPopulation, bool>> GetFilter(string key);
}