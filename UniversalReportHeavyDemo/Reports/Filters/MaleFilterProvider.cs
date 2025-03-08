using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Filters
{
    public class MaleFilterProvider : IFilterProvider<CityPopulation>
    {
        public string Key => "Male";
        public string DisplayName => "Males";

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetAndFilters() =>
            new Expression<Func<CityPopulation, bool>>[] { x => x.Sex == "Male" };

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetOrFilters() =>
            Enumerable.Empty<Expression<Func<CityPopulation, bool>>>();
    }

}
