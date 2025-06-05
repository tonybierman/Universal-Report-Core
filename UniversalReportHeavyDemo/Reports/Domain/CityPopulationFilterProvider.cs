using System.Linq.Expressions;
using UniversalReportCore;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Domain
{
    public class CityPopulationFilterProvider : BaseFilterProvider<CityPopulation>
    {
        private readonly ApplicationDbContext _dbContext;

        public CityPopulationFilterProvider(ApplicationDbContext dbContext)
            : base(new List<Facet<CityPopulation>>())
        {
            _dbContext = dbContext;

            // Dynamically fetch countries
            var genders = _dbContext.CityPopulations
                .Select(p => p.Sex)
                .Distinct()
                .ToList();

            // Create Facet with FacetValues for individual countries
            var genderFacetValues = genders.Select(c =>
                new FacetValue<CityPopulation>(
                    key: c,
                    filter: p => p.Sex == c,
                    displayName: c
                )).ToList();

            // Add dynamic country Facet
            Facets.Add(new Facet<CityPopulation>("Sex", genderFacetValues));

            // Add static "Central America" Facet
            Facets.Add(new Facet<CityPopulation>("CountryOrArea", new List<FacetValue<CityPopulation>>
            {
                new FacetValue<CityPopulation>(
                    key: "Central America",
                    filter: p =>
                        p.CountryOrArea == "Belize" ||
                        p.CountryOrArea == "Costa Rica" ||
                        p.CountryOrArea == "El Salvador" ||
                        p.CountryOrArea == "Guatemala" ||
                        p.CountryOrArea == "Honduras" ||
                        p.CountryOrArea == "Nicaragua" ||
                        p.CountryOrArea == "Panama",
                    displayName: "Central America"
                ),
                new FacetValue<CityPopulation>(
                    key: "South America",
                    filter: p =>
                        p.CountryOrArea == "Brazil",
                    displayName: "South America"
                ),
                new FacetValue<CityPopulation>(
                    key: "North America",
                    filter: p =>
                        p.CountryOrArea == "Canada",
                    displayName: "North America"
                )
            }));
        }
    }
}