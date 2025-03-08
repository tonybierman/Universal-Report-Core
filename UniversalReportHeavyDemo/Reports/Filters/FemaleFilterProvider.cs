using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Filters
{
    public class FemaleFilterProvider : IFilterProvider<CityPopulation>
    {
        public string Key => "Female";
        public string DisplayName => "Females";

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetAndFilters() =>
            new Expression<Func<CityPopulation, bool>>[] { x => x.Sex == "Female" };

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetOrFilters() =>
            Enumerable.Empty<Expression<Func<CityPopulation, bool>>>();
    }

}
