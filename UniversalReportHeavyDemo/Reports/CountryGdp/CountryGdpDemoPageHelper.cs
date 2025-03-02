using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversalReport.Services;
using UniversalReportCore;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Ui;
using UniversalReportHeavyDemo.Data;
using UniversalReportHeavyDemo.ViewModels;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
{
    public class CountryGdpDemoPageHelper : PageHelperBase<NationalGdp, NationalGdpViewModel>
    {
        private ApplicationDbContext _dbContext;

        public CountryGdpDemoPageHelper(
            IUniversalReportService reportService,
            IReportColumnFactory reportColumnFactory,
            IQueryFactory<NationalGdp> queryFactory,
            ApplicationDbContext dbContext,
            IMapper mapper) : base(reportService, reportColumnFactory, queryFactory, mapper)
        {
            _dbContext = dbContext;
            DefaultSort = "CountryNameAsc";
        }

        private IQueryable<NationalGdp> GetLatestNationalGdp(IQueryable<NationalGdp> query)
        {
            var latestYears = query
                .Where(cp => cp.Year.HasValue && !string.IsNullOrEmpty(cp.CountryName))
                .GroupBy(cp => new { cp.CountryName })
                .Select(g => new { g.Key.CountryName, MaxYear = g.Max(cp => cp.Year) });

            return from cp in query
                   join ly in latestYears
                   on new { cp.CountryName, cp.Year } equals new { ly.CountryName, Year = ly.MaxYear }
                   select cp;
        }

        public override async Task<PaginatedList<NationalGdpViewModel>> GetPagedDataAsync(PagedQueryParameters<NationalGdp> parameters, int totalCount)
        {
            IQueryable<NationalGdp> query = GetLatestNationalGdp(_dbContext.NationalGdps);

            return await _reportService.GetPagedAsync<NationalGdp, NationalGdpViewModel>(parameters, totalCount, query);
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

        public override PagedQueryParameters<NationalGdp> CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds)
        {
            return _queryFactory.CreateQueryParameters(queryType, columns, pageIndex, sort, ipp, cohortIds);
        }
    }
}
