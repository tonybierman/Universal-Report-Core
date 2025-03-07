using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Filters
{
    public class CanadaFilterProvider : IFilterProvider<CityPopulation>
    {
        public string Key => "CanadianMales";

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetAndFilters() =>
            new Expression<Func<CityPopulation, bool>>[] { x => x.CountryOrArea == "Canada", x => x.Sex == "Male" };

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetOrFilters() =>
            Enumerable.Empty<Expression<Func<CityPopulation, bool>>>();
    }

}
