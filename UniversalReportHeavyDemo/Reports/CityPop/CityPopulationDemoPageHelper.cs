using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Ui;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace UniversalReportHeavyDemo.Reports.CityPop
{
    public class CityPopulationDemoPageHelper : PageHelperBase<CityPopulation, CityPopulationViewModel>
    {
        private readonly ApplicationDbContext _dbContext;


        public CityPopulationDemoPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<CityPopulation> queryFactory,
            ApplicationDbContext dbContext,
            IFilterProviderRegistry<CityPopulation> filterRegistry,
            FilterFactory<CityPopulation> filterFactory,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, filterRegistry, filterFactory, mapper)
        {
            _dbContext = dbContext;
            DefaultSort = "CityAsc";
        }

        private IQueryable<CityPopulation> GetLatestCityPopulation(IQueryable<CityPopulation> query)
        {
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

        public override async Task<PaginatedList<CityPopulationViewModel>> GetPagedDataAsync(
            PagedQueryParameters<CityPopulation> parameters,
            int totalCount)
        {
            IQueryable<CityPopulation> query = GetLatestCityPopulation(_dbContext.CityPopulations);
            return await _reportService.GetPagedAsync<CityPopulation, CityPopulationViewModel>(
                parameters, totalCount, query);
        }

        public override async Task<ICohort[]?> GetCohortsAsync(int[] cohortIds)
        {
            var cohorts = await _dbContext.CityPopulationCohorts
                            .Where(c => cohortIds.Contains(c.Id))
                            .Cast<ICohort>() // Ensure conversion to ICohort
                            .ToArrayAsync();

            return cohorts.Length > 0 ? cohorts : null;
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
