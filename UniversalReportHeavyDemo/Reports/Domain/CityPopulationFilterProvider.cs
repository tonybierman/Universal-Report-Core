using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;
namespace UniversalReportHeavyDemo.Reports.Domain
{
    public class CityPopulationFilterProvider : BaseFilterProvider<CityPopulation>
    {
        public CityPopulationFilterProvider() : base(new List<Facet<CityPopulation>>
        {
            new("CountryOrArea", new()
            {
                new("Central America", p => 
                    p.CountryOrArea == "Belize" || 
                    p.CountryOrArea == "Costa Rica" || 
                    p.CountryOrArea == "El Salvador" || 
                    p.CountryOrArea == "Guatemala" || 
                    p.CountryOrArea == "Honduras" || 
                    p.CountryOrArea == "Nicaragua" ||
                    p.CountryOrArea == "Panama")
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
