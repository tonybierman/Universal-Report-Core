using AutoMapper;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationPageHelper : BasePageHelper<CityPopulation, CityPopulationViewModel>
    {
        public CityPopulationPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<CityPopulation> queryFactory,
            ApplicationDbContext dbContext,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, dbContext, mapper)
        {
            DefaultSort = "Product.SkuDesc";
        }

        public async override Task<PaginatedList<CityPopulationViewModel>> GetPagedDataAsync(PagedQueryParameters<CityPopulation> parameters)
        {
            //parameters.CohortLogic = (query, cohortIds) =>
            //{
            //    return query.Where(pi =>
            //        _dbContext.Products
            //            .Where(p => p.Id == pi.ProductId)
            //            .SelectMany(p => p.Cohorts)
            //            .Any(c => cohortIds.Contains(c.Id))
            //    );
            //};

            return await _reportService.GetPagedAsync<CityPopulation, CityPopulationViewModel>(parameters);
        }
        public override List<IReportColumnDefinition> GetReportColumns(string slug)
        {
            var columns = _reportColumnFactory.GetReportColumns(slug);

            // Find the first DefaultSort value from sortable columns with a non-null DefaultSort property
            var defaultSortColumn = columns.FirstOrDefault(c => c.IsSortable && c.DefaultSort != null);
            if (defaultSortColumn != null)
            {
                DefaultSort = $"{defaultSortColumn.PropertyName}{defaultSortColumn.DefaultSort}";
            }

            return columns;
        }

        public override PagedQueryParameters<CityPopulation> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return _queryFactory.CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds);
        }
    }
}
