using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportCoreTests.Data;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class PaginatedListTests : IDisposable
    {
        private readonly AcmeDbContext _dbContext;

        public PaginatedListTests()
        {
            // ** Set up In-Memory Database **
            var options = new DbContextOptionsBuilder<AcmeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new AcmeDbContext(options);
            _dbContext.Database.EnsureCreated();

            // ** Seed test data **
            SeedWidgetDatabase();
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
        /// Tests whether `PaginatedList<T>` correctly paginates data.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldReturnPaginatedList()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Page size is 2
            Assert.Equal(3, result.TotalPages); // Total items = 5, so 3 pages of 2 items each
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly calculates page count.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldCalculateTotalPagesCorrectly()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.Equal(3, result.TotalPages); // 5 items with page size of 2 should give 3 total pages
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly handles an empty dataset.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldReturnEmptyList_WhenNoRecords()
        {
            // Arrange
            _dbContext.Widgets.RemoveRange(_dbContext.Widgets);
            await _dbContext.SaveChangesAsync();

            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, result.TotalPages);
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly identifies if there is a next page.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldIndicateNextPageExists()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 1, pageSize: 2);

            // Assert
            Assert.True(result.HasNextPage);
        }

        /// <summary>
        /// Tests whether `PaginatedList<T>` correctly identifies if there is a previous page.
        /// </summary>
        [Fact]
        public async Task CreateAsync_ShouldIndicatePreviousPageExists()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateAsync(query, pageIndex: 2, pageSize: 2);

            // Assert
            Assert.True(result.HasPreviousPage);
        }

        /// <summary>
        /// Tests `CreateMappedAsync` for converting entities into a different type.
        /// </summary>
        [Fact]
        public async Task CreateMappedAsync_ShouldMapEntitiesCorrectly()
        {
            // Arrange
            var query = _dbContext.Widgets.AsQueryable();

            // Act
            var result = await PaginatedList<Widget>.CreateMappedAsync(
                query, pageIndex: 1, pageSize: 2,
                mapFunction: entity => new { entity.City, entity.Value });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Page size is 2
            Assert.Contains(result, x => x.City == "New York");
        }

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
