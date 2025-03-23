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
    public class CityPopDemoPageHelper : PageHelperBase<CityPopulation, CityPopulationViewModel>
    {
        private readonly ApplicationDbContext _dbContext;

        public CityPopDemoPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<CityPopulation> queryFactory,
            ApplicationDbContext dbContext,
            IFilterProvider<CityPopulation> filterProvider,
            FilterFactory<CityPopulation> filterFactory,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, filterProvider, filterFactory, mapper)
        {
            _dbContext = dbContext;
            DefaultSort = "CityAsc";
        }

        public override async Task<PaginatedList<CityPopulationViewModel>> GetPagedDataAsync(
            PagedQueryParameters<CityPopulation> parameters,
            int totalCount)
        {
            return await _reportService.GetPagedAsync<CityPopulation, CityPopulationViewModel>(
                parameters, totalCount);
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
