using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversalReport.Services;
using UniversalReportCore.PagedQueries;
using UniversalReportCoreTests.Data;
using UniversalReportCoreTests.Maps;
using UniversalReportCoreTests.ViewModels;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class UniversalReportServiceTests : IDisposable
    {
        private readonly AcmeDbContext _dbContext;
        private readonly UniversalReportService _reportService;
        private readonly IMapper _mapper;

        public UniversalReportServiceTests()
        {

            // ** Set up AutoMapper **
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WidgetMappingProfile());
            });
            _mapper = mapperConfig.CreateMapper();

            // ** Set up In-Memory Database **
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new AcmeDbContext(options);
            _dbContext.Database.EnsureCreated();

            // ** Seed test data **
            SeedWidgetDatabase();

            // ** Initialize Service **
            _reportService = new UniversalReportService(_dbContext, _mapper);
        }

        /// <summary>
        /// Seeds the in-memory database with test data.
        /// </summary>
        private void SeedWidgetDatabase()
        {
            _dbContext.Widgets.AddRange(new List<Widget>
            {
                new Widget { Id = 1, City = "New York", Value = 8419600, Year = 2020 },
                new Widget { Id = 2, City = "Los Angeles", Value = 3980400, Year = 2020 },
                new Widget { Id = 3, City = "Chicago", Value = 2716000, Year = 2020 },
                new Widget { Id = 4, City = "Houston", Value = 2328000, Year = 2020 },
                new Widget { Id = 5, City = "Phoenix", Value = 1690000, Year = 2020 }
            });

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tests whether `GetPagedAsync` returns a paginated list.
        /// </summary>
        [Fact]
        public async Task GetPagedAsync_ShouldReturnPaginatedList()
        {
            // Arrange
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City", IsSortable = true },
                new ReportColumnDefinition { PropertyName = "Population", IsSortable = true }
            };

            var parameters = new PagedQueryParameters<Widget>(
                columns, pageIndex: 1, sort: "CityAsc", itemsPerPage: 2, cohortIds: null);

            // Act
            var result = await _reportService.GetPagedAsync<Widget, WidgetViewModel>(parameters, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Page size is 2
        }

        /// <summary>
        /// Tests whether `GetPagedAsync` returns results in descending order.
        /// </summary>
        [Fact]
        public async Task GetPagedAsync_ShouldSortResultsCorrectly()
        {
            // Arrange
            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City", IsSortable = true }
            };

            var parameters = new PagedQueryParameters<Widget>(
                columns, pageIndex: 1, sort: "CityDesc", itemsPerPage: 5, cohortIds: null);

            // Act
            var result = await _reportService.GetPagedAsync<Widget, WidgetViewModel>(parameters, 0);
            var sortedCities = result.Select(r => r.City).ToList();

            // Assert
            var expectedCities = new List<string> { "Phoenix", "New York", "Los Angeles", "Houston", "Chicago" };
            Assert.Equal(expectedCities, sortedCities);
        }

        /// <summary>
        /// Tests whether `GetPagedAsync` returns an empty list when there are no records.
        /// </summary>
        [Fact]
        public async Task GetPagedAsync_ShouldReturnEmptyForNoRecords()
        {
            // Arrange
            _dbContext.Widgets.RemoveRange(_dbContext.Widgets);
            await _dbContext.SaveChangesAsync();

            var columns = new IReportColumnDefinition[]
            {
                new ReportColumnDefinition { PropertyName = "City", IsSortable = true }
            };

            var parameters = new PagedQueryParameters<Widget>(
                columns, pageIndex: 1, sort: "CityAsc", itemsPerPage: 10, cohortIds: null);

            // Act
            var result = await _reportService.GetPagedAsync<Widget, WidgetViewModel>(parameters, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        /// <summary>
        /// Tests filtering by date range.
        /// </summary>
        //[Fact]
        //public async Task GetPagedAsync_ShouldFilterByDateRange()
        //{
        //    // Arrange
        //    var columns = new IReportColumnDefinition[]
        //    {
        //        new ReportColumnDefinition { PropertyName = "Year", IsSortable = true }
        //    };

        //    var dateFilter = new DateRangeFilter(
        //        start: new DateTime(2020, 1, 1),
        //        end: new DateTime(2020, 12, 31),
        //        name: "Year"
        //    );

        //    var parameters = new PagedQueryParameters<Widget>(
        //        columns, pageIndex: 1, sort: "YearAsc", itemsPerPage: 10, cohortIds: null)
        //    {
        //        DateFilter = dateFilter
        //    };

        //    // Act
        //    var result = await _reportService.GetPagedAsync<Widget, WidgetViewModel>(parameters);

        //    // Assert
        //    Assert.All(result, item =>
        //        Assert.InRange(item.Year.GetValueOrDefault(), dateFilter.StartDate.Year, dateFilter.EndDate.Year));
        //}

        /// <summary>
        /// Cleans up the in-memory database after each test.
        /// </summary>
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
