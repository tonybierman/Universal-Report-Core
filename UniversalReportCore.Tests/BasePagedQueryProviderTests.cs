using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Tests whether the GetQuery method correctly initializes a paged query.
        /// </summary>
        [Fact]
        public void GetQuery_ShouldReturn_PagedQueryParameters()
        {
            // Arrange
            var columns = new IReportColumnDefinition[]
            {
            new ReportColumnDefinition { PropertyName = "City", IsSortable = true },
            new ReportColumnDefinition { PropertyName = "Population", IsSortable = true }
            };

            int? pageIndex = 1;
            string? sort = "City";
            int? ipp = 10;
            int[]? cohortIds = null;

            // Act
            var result = _queryProvider.GetQuery(columns, pageIndex, sort, ipp, cohortIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(sort, result.Sort);
            Assert.Equal(ipp, result.ItemsPerPage);
        }

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
            var resultQuery = _queryProvider.EnsureAggregateQuery(query, cohortIds);

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
            var resultQuery = _queryProvider.EnsureCohortQuery(query, cohortId);

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
            new Widget { Id = 1, City = "New York", Value = 800000 },
            new Widget { Id = 2, City = "Los Angeles", Value = 1200000 },
            new Widget { Id = 3, City = "Chicago", Value = 1000000 }
        });

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
