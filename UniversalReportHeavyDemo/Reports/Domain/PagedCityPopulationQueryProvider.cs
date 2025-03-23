using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.Domain
{
    public class PagedCityPopulationQueryProvider : BasePagedQueryProvider<CityPopulation>
    {
        protected readonly ApplicationDbContext _dbContext;

        public PagedCityPopulationQueryProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;    
        }

        public override string Slug => throw new NotImplementedException();

        protected async override Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<CityPopulation> query)
        {
            return new Dictionary<string, dynamic>();
        }

        public override IQueryable<CityPopulation> EnsureAggregateQuery(IQueryable<CityPopulation> query, int[]? cohortIds)
        {
            if (cohortIds == null || cohortIds.Length == 0)
            {
                return query;  // No filtering needed
            }

            // Fetch all CityPopulationsIds associated with the specified cohortIds
            var ids = _dbContext.CityPopulations
                .Where(p => p.CityPopulationCohorts.Any(c => cohortIds.Contains(c.Id)))
                .Select(p => p.Id)
                .ToList();

            // Filter CityPopulations where Id is not null and matches one of the ids
            return query.Where(entity => ids.Contains(entity.Id));
        }

        public override IQueryable<CityPopulation> EnsureCohortQuery(IQueryable<CityPopulation> query, int cohortId)
        {
            return from entity in query
                   join cityPop in _dbContext.CityPopulations on entity.Id equals cityPop.Id
                   where cityPop.CityPopulationCohorts.Any(c => c.Id == cohortId)
                   select entity;
        }
    }
}
