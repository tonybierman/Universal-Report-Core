using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportDemo.Data;
using UniversalReportDemo.ViewModels;

namespace UniversalReportDemo.Reports.CityPop
{
    public class CityPopulationDemoPageHelper : BasePageHelper<CityPopulation, CityPopulationViewModel>
    {
        public CityPopulationDemoPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<CityPopulation> queryFactory,
            ApplicationDbContext dbContext,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, dbContext, mapper)
        {
            DefaultSort = "CityAsc";
        }

        private IQueryable<CityPopulation> GetLatestCityPopulation(IQueryable<CityPopulation> query)
        {
            var latestYears = query
                .Where(cp => cp.Year.HasValue && !string.IsNullOrEmpty(cp.Sex) && !string.IsNullOrEmpty(cp.City))
                .GroupBy(cp => new { cp.City, cp.Sex })
                .Select(g => new { g.Key.City, g.Key.Sex, MaxYear = g.Max(cp => cp.Year) });

            return from cp in query
                   join ly in latestYears
                   on new { cp.City, cp.Sex, cp.Year } equals new { ly.City, ly.Sex, Year = ly.MaxYear }
                   select cp;
        }

        public override async Task<PaginatedList<CityPopulationViewModel>> GetPagedDataAsync(PagedQueryParameters<CityPopulation> parameters)
        {
            if (parameters.CohortIds.Any())
            {
                //parameters.CohortLogic = (query, cohortIds) =>
                //{
                //    return query.Where(pi =>
                //        _dbContext.CityPopulations
                //            .Where(p => p.Id == pi.Id)
                //            .SelectMany(p => p.Cohorts)
                //            .Any(c => cohortIds.Contains(c.Id))
                //    );
                //};
            }

            IQueryable<CityPopulation> query = GetLatestCityPopulation(_dbContext.CityPopulations);

            return await _reportService.GetPagedAsync<CityPopulation, CityPopulationViewModel>(parameters, query);
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
