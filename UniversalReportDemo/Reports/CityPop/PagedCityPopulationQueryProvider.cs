using UniversalReportCore.PagedQueries;
using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CityPop
{
    public class PagedCityPopulationQueryProvider : BasePagedQueryProvider<CityPopulation>
    {
        private readonly ApplicationDbContext _dbContext;

        public PagedCityPopulationQueryProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;    
        }

        public override string Slug => throw new NotImplementedException();

        protected async override Task<Dictionary<string, dynamic>> EnsureMeta(IQueryable<CityPopulation> query)
        {
            return new Dictionary<string, dynamic>();
        }

        //public override IQueryable<CityPopulation> EnsureAggregateQuery(IQueryable<CityPopulation> query, int[]? cohortIds)
        //{
        //    if (cohortIds == null || cohortIds.Length == 0)
        //    {
        //        return query;  // No filtering needed
        //    }

        //    // Fetch all ProductIds associated with the specified cohortIds
        //    var productIds = _dbContext.Products
        //        .Where(p => p.Cohorts.Any(c => cohortIds.Contains(c.Id)))
        //        .Select(p => p.Id)
        //        .ToList();

        //    // Filter CityPopulations where ProductId is not null and matches one of the ProductIds
        //    return query.Where(entity => entity.ProductId.HasValue && productIds.Contains(entity.ProductId.Value));
        //}

        //public override IQueryable<CityPopulation> EnsureCohortQuery(IQueryable<CityPopulation> query, int cohortId)
        //{
        //    return from entity in query
        //           join product in _dbContext.Products on entity.ProductId equals product.Id
        //           where product.Cohorts.Any(c => c.Id == cohortId)
        //           select entity;
        //}
    }
}
