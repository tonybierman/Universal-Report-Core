using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Ui;
using UniversalReportCoreTests.Data;
using UniversalReportCoreTests.Reports;
using UniversalReportCoreTests.ViewModels;

namespace UniversalReportCore.Tests.Reports.Acme
{
    public class AcmeDemoPageHelper : PageHelperBase<Widget, WidgetViewModel>
    {
        AcmeDbContext _dbContext;
        public AcmeDemoPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<Widget> queryFactory,
            AcmeDbContext dbContext,
            IFilterProvider<Widget> filterProvider,
            FilterFactory<Widget> filterFactory,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, filterProvider, filterFactory, mapper)
        {
            DefaultSort = "CityAsc";
            _dbContext = dbContext;
        }

        private IQueryable<Widget> GetLatestCityPopulation(IQueryable<Widget> query)
        {
            //var latestYears = query
            //    .Where(cp => cp.Year.HasValue && !string.IsNullOrEmpty(cp.Sex) && !string.IsNullOrEmpty(cp.City))
            //    .GroupBy(cp => new { cp.City, cp.Sex })
            //    .Select(g => new { g.Key.City, g.Key.Sex, MaxYear = g.Max(cp => cp.Year) });

            //return from cp in query
            //       join ly in latestYears
            //       on new { cp.City, cp.Sex, cp.Year } equals new { ly.City, ly.Sex, Year = ly.MaxYear }
            //       select cp;

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

        public override async Task<PaginatedList<WidgetViewModel>> GetPagedDataAsync(PagedQueryParameters<Widget> parameters, int totalCount)
        {
            IQueryable<Widget> query = GetLatestCityPopulation(_dbContext.Widgets);

            return await _reportService.GetPagedAsync<Widget, WidgetViewModel>(parameters, totalCount, query);
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

        public override PagedQueryParameters<Widget> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds, string[]? filterKeys)
        {
            return _queryFactory.CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds, EnsureFilterConfig(filterKeys));
        }
    }
}
