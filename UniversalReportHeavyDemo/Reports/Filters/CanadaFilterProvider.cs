using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Filters
{
    public class CanadaFilterProvider : IFilterProvider<CityPopulation>
    {
        public string Key => "Canada";
        public string DisplayName => "Canada";

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetAndFilters() =>
            new Expression<Func<CityPopulation, bool>>[] { x => x.CountryOrArea == "Canada" };

        public IEnumerable<Expression<Func<CityPopulation, bool>>> GetOrFilters() =>
            Enumerable.Empty<Expression<Func<CityPopulation, bool>>>();
    }

}
