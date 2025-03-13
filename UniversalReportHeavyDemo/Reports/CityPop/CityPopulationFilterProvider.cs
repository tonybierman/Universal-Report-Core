using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;
namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopulationFilterProvider : BaseFilterProvider<CityPopulation>
    {
        public CityPopulationFilterProvider() : base(new List<Facet<CityPopulation>>
        {
            new("CountryOrArea", new()
            {
                new("Canada", p => p.CountryOrArea == "Canada"),
                new("Mexico", p => p.CountryOrArea == "Mexico"),
                new("Pakistan", p => p.CountryOrArea == "Pakistan"),
                new("Japan", p => p.CountryOrArea == "Japan"),
            }),
            new("Sex", new()
            {
                new("Male", p => p.Sex == "Male"),
                new("Female", p => p.Sex == "Female"),
            })
        })
        { }
    }
}
