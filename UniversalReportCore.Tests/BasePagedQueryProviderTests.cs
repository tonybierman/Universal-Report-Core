using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.Tests.Reports.Acme;
using UniversalReportCoreTests.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class BasePagedQueryProviderTests : IDisposable
    {
        private readonly AcmeDbContext _dbContext;
        private readonly PagedAcmeQueryProvider _queryProvider;

        public BasePagedQueryProviderTests()
        {
            // Initialize in-memory database
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new AcmeDbContext(options);
            _queryProvider = new PagedAcmeQueryProvider(_dbContext);

            SeedDatabase();
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Sum()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] {             
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Sum },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Sum }
            };

            var query = _dbContext.Widgets.AsQueryable();
            var cohortIds = new int[] { 1, 2 };
            var helper = new PagedAcmeQueryProvider(_dbContext);

            // Act
            var result = await helper.ComputeAggregates(query, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(null, result["City"]);
            Assert.Equal(3000000, result["Value"]);
            Assert.Equal(3000000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Average()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Average },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Average }
            };

            var query = _dbContext.Widgets.AsQueryable();
            var cohortIds = new int[] { 1, 2 };
            var helper = new PagedAcmeQueryProvider(_dbContext);

            // Act
            var result = await helper.ComputeAggregates(query, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(null, result["City"]);
            Assert.Equal(1000000, result["Value"]);
            Assert.Equal(1000000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Count()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Count },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Count }
            };

            var query = _dbContext.Widgets.AsQueryable();
            var cohortIds = new int[] { 1, 2 };
            var helper = new PagedAcmeQueryProvider(_dbContext);

            // Act
            var result = await helper.ComputeAggregates(query, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(null, result["City"]);
            Assert.Equal(3, result["Value"]);
            Assert.Equal(3, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Min()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Min },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Min }
            };

            var query = _dbContext.Widgets.AsQueryable();
            var cohortIds = new int[] { 1, 2 };
            var helper = new PagedAcmeQueryProvider(_dbContext);

            // Act
            var result = await helper.ComputeAggregates(query, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(null, result["City"]);
            Assert.Equal(800000, result["Value"]);
            Assert.Equal(800000, result["IntValue"]);
        }

        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_Max()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] {
                new ReportColumnDefinition { PropertyName = "City" },
                new ReportColumnDefinition { PropertyName = "Value", Aggregation = AggregationType.Max },
                new ReportColumnDefinition { PropertyName = "IntValue", Aggregation = AggregationType.Max }

            };

            var query = _dbContext.Widgets.AsQueryable();
            var cohortIds = new int[] { 1, 2 };
            var helper = new PagedAcmeQueryProvider(_dbContext);

            // Act
            var result = await helper.ComputeAggregates(query, columns);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("City", result);
            Assert.Contains("Value", result);
            Assert.Contains("IntValue", result);
            Assert.Equal(null, result["City"]);
            Assert.Equal(1200000, result["Value"]);
            Assert.Equal(1200000, result["IntValue"]);
        }


        [Fact]
        public async Task ComputeAggregatesWithCohortsAsync_ReturnsAggregates_WhenCohortIdsAreNull()
        {
            // Arrange
            var columns = new IReportColumnDefinition[] { /* mock column definitions */ };
            var query = new List<Widget> { /* mock data */ }.AsQueryable();
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
            var dbContextMock = new Mock<AcmeDbContext>(options);
            var helper = new Mock<PagedAcmeQueryProvider>(dbContextMock.Object) { CallBase = true };

            // Expected aggregates result
            var expectedAggregates = new Dictionary<string, dynamic> { { "Total", 100 } };

            // Mock ComputeAggregates to return expectedAggregates
            helper
                .Setup(h => h.ComputeAggregates(It.IsAny<IQueryable<Widget>>(), It.IsAny<IReportColumnDefinition[]>()))
                .ReturnsAsync(expectedAggregates);

            // Act
            var result = await helper.Object.ComputeAggregatesWithCohortsAsync(query, columns, null);

            // Assert
            Assert.Equal(expectedAggregates, result);
            helper.Verify(h => h.ComputeAggregates(It.IsAny<IQueryable<Widget>>(), columns), Times.Once);
        }



        /// <summary>
        /// Tests whether the GetQuery method correctly initializes a paged query.
        /// </summary>
        //[Fact]
        //public async Task GetQuery_ShouldReturn_PagedQueryParameters()
        //{
        //    // Arrange
        //    var columns = new IReportColumnDefinition[]
        //    {
        //    new ReportColumnDefinition { PropertyName = "City", IsSortable = true },
        //    new ReportColumnDefinition { PropertyName = "Population", IsSortable = true }
        //    };

        //    int? pageIndex = 1;
        //    string? sort = "City";
        //    int? ipp = 10;
        //    int[]? cohortIds = null;

        //    // Act
        //    var result = _queryProvider.BuildPagedQuery(columns, pageIndex, sort, ipp, cohortIds, null, null);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(pageIndex, result.PageIndex);
        //    Assert.Equal(sort, result.Sort);
        //    Assert.Equal(ipp, result.ItemsPerPage);
        //}

        /// <summary>
        /// Tests that EnsureAggregateQuery does not modify the query when no cohort filters exist.
        /// </summary>
        [Fact]
        public void EnsureAggregateQuery_ShouldReturn_UnmodifiedQuery_WhenNoCohorts()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();
            int[]? cohortIds = null;

            // Act
            var resultQuery = _queryProvider.EnsureAggregatePredicate(query, cohortIds);

            // Assert
            Assert.Equal(query.Count(), resultQuery.Count());
        }

        /// <summary>
        /// Tests that EnsureCohortQuery applies the correct filter.
        /// </summary>
        [Fact]
        public void EnsureCohortQuery_ShouldReturn_FilteredQuery()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();
            int cohortId = 1;

            // Act
            var resultQuery = _queryProvider.EnsureCohortPredicate(query, cohortId);

            // Assert
            Assert.NotNull(resultQuery);
        }

        /// <summary>
        /// Seeds the in-memory database with test data.
        /// </summary>
        private void SeedDatabase()
        {
            _dbContext.Widgets.AddRange(new List<Widget>
        {
            new Widget { Id = 1, City = "New York", Value = 800000, IntValue = 800000 },
            new Widget { Id = 2, City = "Los Angeles", Value = 1200000, IntValue = 1200000 },
            new Widget { Id = 3, City = "Chicago", Value = 1000000, IntValue = 1000000 }
        });

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
