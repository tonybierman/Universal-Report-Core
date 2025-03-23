using Microsoft.EntityFrameworkCore;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.Reports.Domain;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopDemoQueryProvider : PagedCityPopulationQueryProvider
    {
        public override string Slug => "CityPopDemo";

        public CityPopDemoQueryProvider(ApplicationDbContext dbContext) : base(dbContext) { }

        public override IQueryable<CityPopulation> EnsureReportQuery()
        {
            IQueryable<CityPopulation> query = _dbContext.CityPopulations;

            var latestYears = query
                .Where(cp => cp.Year.HasValue && cp.City != null && cp.Sex != null)
                .GroupBy(cp => new { cp.City, cp.Sex })
                .Select(g => new { g.Key.City, g.Key.Sex, MaxYear = g.Max(cp => cp.Year) })
                .AsNoTracking();

            var optimizedQuery = query
                .Join(latestYears,
                    cp => new { cp.City, cp.Sex, cp.Year },
                    ly => new { ly.City, ly.Sex, Year = ly.MaxYear },
                    (cp, ly) => cp)
                .AsNoTracking();

            return optimizedQuery;
        }
    }
}
